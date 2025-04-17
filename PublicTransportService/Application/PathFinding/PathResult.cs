namespace PublicTransportService.Application.PathFinding;

/// <summary>
/// Represents the result of an individual user's computed
/// travel path as part of a proposed group route.
/// </summary>
/// <param name="EarliestDeparture">
/// The earliest time the user should depart
/// from their location to follow the path.
/// </param>
/// <param name="Segments">
/// The ordered segments composing the user's travel path
/// from their origin to the shared destination.
/// </param>
public record PathResult(
    DateTime EarliestDeparture,
    List<PathSegment> Segments);
