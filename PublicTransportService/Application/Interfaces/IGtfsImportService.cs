namespace PublicTransportService.Application.Interfaces;

/// <summary>
/// Defines a contract for importing GTFS data into the database.
/// </summary>
public interface IGtfsImportService
{
    /// <summary>
    /// Imports GTFS data into the database by downloading
    /// the feed from a remote source or using a local zip file.
    /// </summary>
    /// <param name="localZipPath">
    /// Optional path to a local GTFS zip file.
    /// If specified, the file will be used instead of downloading from a remote source.
    /// If <see langword="null"/>, the GTFS feed will be fetched from the default URL.
    /// </param>
    /// <param name="chunkSize">
    /// Optional size of data batches to be inserted into the database.
    /// A higher value may speed up the process but consume more memory.
    /// </param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ImportAsync(string? localZipPath, int chunkSize);
}
