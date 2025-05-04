using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace PublicTransportService.Infrastructure.Importing;

/// <summary>
/// Provides a static helper for generating record chunks from a CSV file.
/// </summary>
public static class CsvChunkGenerator
{
    /// <summary>
    /// Reads a CSV file and maps it to a sequence of entity chunks.
    /// </summary>
    /// <typeparam name="TCsv">The CSV record type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="path">The CSV file path.</param>
    /// <param name="chunkSize">Number of items per chunk.</param>
    /// <param name="map">Mapping function.</param>
    /// <returns>An async enumerable of entity chunks.</returns>
    public static async IAsyncEnumerable<List<TEntity>> GenerateChunksAsync<TCsv, TEntity>(
        string path,
        int chunkSize,
        Func<IAsyncEnumerable<TCsv>, IAsyncEnumerable<TEntity>> map)
        where TCsv : class
        where TEntity : class
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
        });

        var batch = new List<TEntity>(chunkSize);
        await foreach (var entity in map(csv.GetRecordsAsync<TCsv>()))
        {
            batch.Add(entity);
            if (batch.Count >= chunkSize)
            {
                yield return batch;
                batch = new List<TEntity>(chunkSize);
            }
        }

        if (batch.Count > 0)
        {
            yield return batch;
        }
    }
}
