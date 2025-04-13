using PublicTransportService.Domain.Entities;

namespace PublicTransportService.Domain.Interfaces;

/// <summary>
/// Defines a contract for a repository that manages trip data.
/// </summary>
public interface ITripRepository
{
    /// <summary>
    /// Retrieves a trip by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the trip to retrieve.</param>
    /// <returns>The trip associated with the specified identifier, or null if not found.</returns>
    Task<Trip?> GetByIdAsync(string id);
}
