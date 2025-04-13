namespace PublicTransportService.Application.PathFinding;

/// <summary>
/// Represents the result of a computed path
/// for a user within a proposed travel scenario.
/// </summary>
/// <param name="EarliestDeparture">
/// The earliest time the user should depart
/// from their location to follow the path.
/// </param>
/// <param name="Segments">
/// The ordered list of segments that make up
/// the user's path from their origin to the destination.
/// </param>
public record PathResult(
    DateTime EarliestDeparture,
    List<PathSegment> Segments);
