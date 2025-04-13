namespace PublicTransportService.Infrastructure.PathFinding;

/// <summary>
/// Represents a lightweight stop time used in pathfinding algorithms.
/// </summary>
/// <param name="TripId">The unique identifier of the trip.</param>
/// <param name="StopId">The ID of the stop.</param>
/// <param name="ArrivalTime">The arrival time in seconds since midnight.</param>
/// <param name="DepartureTime">The departure time in seconds since midnight.</param>
/// <param name="StopSequence">The sequence number of the stop within the trip.</param>
internal sealed record PathFindingStopTime(
    string TripId,
    string StopId,
    DateTime ArrivalTime,
    DateTime DepartureTime,
    int StopSequence);
