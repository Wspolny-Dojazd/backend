namespace PublicTransportService.Infrastructure.PathFinding;

/// <summary>
/// Represents a lightweight stop used in pathfinding algorithms.
/// </summary>
/// <param name="Id">The unique identifier of the stop.</param>
/// <param name="LogicalId">The logical identifier of the stop, used for grouping or categorization.</param>
/// <param name="Latitude">The latitude of the stop.</param>
/// <param name="Longitude">The longitude of the stop.</param>
internal sealed record PathFindingStop(string Id, string LogicalId, double Latitude, double Longitude);
