using Application.DTOs.Path;
using PublicTransportService.Application.PathFinding;
using PublicTransportService.Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for assembling user paths from raw path planning results.
/// </summary>
public interface IPathAssembler
{
    /// <summary>
    /// Assembles user paths based on path planning results and stop metadata.
    /// </summary>
    /// <param name="paths">The computed path results, mapped by user ID.</param>
    /// <param name="stopLookup">A lookup containing stop metadata.</param>
    /// <param name="userLocations">A lookup containing user locations.</param>
    /// <param name="destination">The final destination of the path.</param>
    /// <param name="arrivalTime">The arrival time.</param>
    /// <returns>The assembled user paths.</returns>
    Task<IEnumerable<UserPathDto>> AssemblePaths(
        Dictionary<Guid, PathResult> paths,
        IReadOnlyDictionary<string, Stop> stopLookup,
        IReadOnlyDictionary<Guid, (double Latitude, double Longitude)> userLocations,
        (double Latitude, double Longitude) destination,
        DateTime arrivalTime);
}
