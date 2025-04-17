using PublicTransportService.Application.PathFinding;

namespace PublicTransportService.Application.Interfaces;

/// <summary>
/// Defines a contract for services that compute public transport paths
/// for multiple users converging on a common destination.
/// </summary>
public interface IPathPlanningService
{
    /// <summary>
    /// Computes multiple path sets for a group of users traveling
    /// to a shared destination by a specified arrival time.
    /// </summary>
    /// <param name="destLatitude">The latitude of the destination point.</param>
    /// <param name="destLongitude">The longitude of the destination point.</param>
    /// <param name="arrivalTime">The desired arrival time at the destination.</param>
    /// <param name="userLocations">
    /// The starting locations of the users, each represented
    /// by their unique identifier and geographic coordinates.
    /// </param>
    /// <returns>
    /// The alternative path sets. Each dictionary in the collection represents
    /// one possible travel scenario for the group, mapping user identifiers
    /// to their individual <see cref="PathResult"/>.
    /// </returns>
    Task<IEnumerable<Dictionary<Guid, PathResult>>> ComputeGroupPathsAsync(
        double destLatitude,
        double destLongitude,
        DateTime arrivalTime,
        IEnumerable<(Guid UserId, double Latitude, double Longitude)> userLocations);
}
