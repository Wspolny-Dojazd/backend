using PublicTransportService.Domain.Entities;

namespace PublicTransportService.Domain.Interfaces;

/// <summary>
/// Defines a contract for a repository that manages public transport stops.
/// </summary>
public interface IStopRepository
{
    /// <summary>
    /// Retrieves a stop by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the stop.</param>
    /// <returns>The matching stop if found; otherwise, <see langword="null"/>.</returns>
    Task<Stop?> GetByIdAsync(string id);

    /// <summary>
    /// Retrieves a list of stops by their unique identifiers.
    /// </summary>
    /// <param name="ids">The unique identifiers for the stops to retrieve.</param>
    /// <returns>The stops matching the specified identifiers.</returns>
    Task<List<Stop>> GetByIdsAsync(IEnumerable<string> ids);

    /// <summary>
    /// Retrieves the logical ID of the stop nearest to the specified coordinates.
    /// </summary>
    /// <param name="latitude">The latitude of the location.</param>
    /// <param name="longitude">The longitude of the location.</param>
    /// <returns>The logical ID of the nearest stop.</returns>
    Task<string> GetNearestLogicalIdStopAsync(double latitude, double longitude);
}
