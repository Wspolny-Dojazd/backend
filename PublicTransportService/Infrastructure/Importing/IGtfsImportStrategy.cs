namespace PublicTransportService.Infrastructure.Importing;

/// <summary>
/// Represents a strategy for importing GTFS entities into a specific destination table.
/// </summary>
internal interface IGtfsImportStrategy
{
    /// <summary>
    /// Gets a value indicating whether the import operation requires a transaction.
    /// </summary>
    bool RequiresTransaction { get; }

    /// <summary>
    /// Imports the given GTFS entity data into the specified database table.
    /// </summary>
    /// <typeparam name="TCsv">The type representing a CSV row.</typeparam>
    /// <typeparam name="TEntity">The target entity type.</typeparam>
    /// <param name="path">The path to the GTFS CSV file.</param>
    /// <param name="map">A function mapping a CSV record to an entity.</param>
    /// <param name="chunkSize">The number of records to insert per batch.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ImportAsync<TCsv, TEntity>(
        string path,
        Func<IAsyncEnumerable<TCsv>, IAsyncEnumerable<TEntity>> map,
        int chunkSize)
        where TCsv : class
        where TEntity : class;
}
