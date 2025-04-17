using PublicTransportService.Domain.Entities;

namespace PublicTransportService.Domain.Interfaces;

/// <summary>
/// Defines a contract for a repository that manages route data.
/// </summary>
public interface IRouteRepository
{
    /// <summary>
    /// Retrieves a route by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the route to retrieve.</param>
    /// <returns>
    /// The route associated with the specified identifier if found;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    Task<Route?> GetByIdAsync(string id);
}
