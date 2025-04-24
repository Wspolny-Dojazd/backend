namespace PublicTransportService.Infrastructure.PathFinding.Raptor;

/// <summary>
/// Defines a contract for a cache that holds preloaded RAPTOR pathfinding data.
/// </summary>
internal interface IRaptorDataCache
{
    /// <summary>
    /// Initializes the cache by loading data from the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    Task InitializeAsync();

    /// <summary>
    /// Gets the RAPTOR context containing preprocessed data.
    /// </summary>
    /// <returns>The populated <see cref="RaptorContext"/> instance.</returns>
    RaptorContext GetContext();
}
