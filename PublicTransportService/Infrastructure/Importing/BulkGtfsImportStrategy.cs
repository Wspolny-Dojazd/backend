using System.Diagnostics;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublicTransportService.Infrastructure.Data;

namespace PublicTransportService.Infrastructure.Importing;

/// <summary>
/// Represents a strategy for importing GTFS data using bulk insert operations.
/// </summary>
internal sealed class BulkGtfsImportStrategy(PTSDbContext context, ILogger<BulkGtfsImportStrategy> logger)
    : IGtfsImportStrategy
{
    /// <inheritdoc/>
    public bool RequiresTransaction => false;

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

        var tmpTableName = $"{tableName}_tmp";
        logger.LogInformation("Preparing temporary table {Table}", tmpTableName);
        var dropSql = $"DROP TABLE IF EXISTS `{tmpTableName}`;";
        var createSql = $"CREATE TABLE `{tmpTableName}` LIKE `{tableName}`;";
        _ = await context.Database.ExecuteSqlRawAsync(dropSql);
        _ = await context.Database.ExecuteSqlRawAsync(createSql);

        var processed = 0;
        logger.LogInformation("Preparing and inserting chunks into {Table}", tmpTableName);
        var chunks = CsvChunkGenerator.GenerateChunksAsync(path, chunkSize, map);
        await foreach (var chunk in chunks)
        {
            await context.BulkInsertAsync(chunk, new BulkConfig
            {
                CustomDestinationTableName = tmpTableName,
                SetOutputIdentity = false,
                EnableStreaming = true,
                PreserveInsertOrder = false,
                UseTempDB = false,
            });

            processed += chunk.Count;
            Console.Write($"\rInserted {processed} records into {tmpTableName}".PadRight(80));
        }

        Console.Write('\r');
        logger.LogInformation("Bulk inserted all {Count} records into {Table}", processed, tmpTableName);

        var renameSql = $"RENAME TABLE `{tableName}` TO `{tableName}_old`, `{tmpTableName}` TO `{tableName}`;";
        var dropOldSql = $"DROP TABLE `{tableName}_old`;";
        _ = await context.Database.ExecuteSqlRawAsync(renameSql);
        _ = await context.Database.ExecuteSqlRawAsync(dropOldSql);
        logger.LogInformation("Swapped tables: {TmpTable} → {Table} and dropped old", tmpTableName, tableName);

        totalStopwatch.Stop();
        logger.LogInformation(
            "Total bulk insert duration for {Table}: {Time:N2} seconds",
            tableName,
            totalStopwatch.Elapsed.TotalSeconds);
    }
}
