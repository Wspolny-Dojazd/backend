namespace PublicTransportService.Application.PathFinding;

/// <summary>
/// Represents an individual segment of a user's travel path,
/// such as a transit ride or a walking transfer between stops.
/// </summary>
/// <param name="FromStopId">The identifier of the stop where the segment begins.</param>
/// <param name="ToStopId">The identifier of the stop where the segment ends.</param>
/// <param name="TripId">
/// The identifier of the transit trip associated with the segment,
/// or <see langword="null"/> if the segment represents a walking transfer.
/// </param>
/// <param name="DepartureTime">The scheduled departure time from the origin stop.</param>
/// <param name="ArrivalTime">The scheduled arrival time at the destination stop.</param>
public record PathSegment(
    string FromStopId,
    string ToStopId,
    string? TripId,
    DateTime DepartureTime,
    DateTime ArrivalTime);
