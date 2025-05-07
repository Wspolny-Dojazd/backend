using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublicTransportService.Infrastructure.Data;

namespace PublicTransportService.Infrastructure.Importing;

/// <summary>
/// Represents a strategy for importing GTFS chunks into the database using Entity Framework Core.
/// </summary>
internal sealed class EfCoreGtfsImportStrategy(PTSDbContext context, ILogger<EfCoreGtfsImportStrategy> logger)
    : IGtfsImportStrategy
{
    /// <inheritdoc/>
    public bool RequiresTransaction => true;

    /// <inheritdoc/>
    public async Task ImportAsync<TCsv, TEntity>(
        string path,
        Func<IAsyncEnumerable<TCsv>, IAsyncEnumerable<TEntity>> map,
        int chunkSize)
        where TCsv : class
        where TEntity : class
    {
        var totalStopwatch = Stopwatch.StartNew();

        var tableName = context.Model.FindEntityType(typeof(TEntity))?.GetTableName();
        if (string.IsNullOrEmpty(tableName))
        {
            logger.LogWarning(
                "Table name for {EntityLabel} not found in the model. Using default name.",
                tableName = typeof(TEntity).Name);
        }

        logger.LogInformation("Clearing {Table}", tableName);

        var sectionStopwatch = Stopwatch.StartNew();
        _ = await context.Set<TEntity>().ExecuteDeleteAsync();
        _ = await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        sectionStopwatch.Stop();

        logger.LogInformation(
            "Cleared {Table} in {Time:N2} seconds",
            tableName,
            sectionStopwatch.Elapsed.TotalSeconds);

        logger.LogInformation("Generating chunks for {Table}", tableName);

        sectionStopwatch = Stopwatch.StartNew();
        var chunks = await CsvChunkGenerator.GenerateChunksAsync(path, chunkSize, map).ToListAsync();
        var allRecords = chunks.Sum(c => c.Count);
        var chunkTimes = new Queue<TimeSpan>();
        sectionStopwatch.Stop();

        logger.LogInformation(
            "Generated {Count} chunks with {TotalRecords} records for {Table} in {Time:N2} seconds",
            chunks.Count,
            allRecords,
            tableName,
            sectionStopwatch.Elapsed.TotalSeconds);

        const int ChunkHistoryLimit = 10;
        var processed = 0;
        foreach (var chunk in chunks)
        {
            var chunkStopwatch = Stopwatch.StartNew();

            await context.AddRangeAsync(chunk);
            _ = await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            chunkStopwatch.Stop();

            if (chunkTimes.Count >= ChunkHistoryLimit)
            {
                _ = chunkTimes.Dequeue();
            }

            chunkTimes.Enqueue(chunkStopwatch.Elapsed);

            processed += chunk.Count;

            var avgMs = chunkTimes.Average(t => t.TotalMilliseconds);
            var remainingRecords = allRecords - processed;
            var remainingChunks = (int)Math.Ceiling((double)remainingRecords / chunkSize);
            var eta = TimeSpan.FromMilliseconds(avgMs * remainingChunks);

            Console.Write(
                $"\rInserted {processed}/{allRecords} records to {tableName}..." +
                $" ETA: {eta:hh\\:mm\\:ss}".PadRight(80));
        }

        Console.Write("\r");
        totalStopwatch.Stop();

        logger.LogInformation(
            "Inserted all {Count} records into {Table} in {Time:N2} seconds",
            processed,
            tableName,
            totalStopwatch.Elapsed.TotalSeconds);
    }
}
