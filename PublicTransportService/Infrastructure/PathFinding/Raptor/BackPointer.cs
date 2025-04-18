namespace PublicTransportService.Infrastructure.PathFinding.Raptor;

/// <summary>
/// Represents a back pointer used during path reconstruction in the RAPTOR algorithm.
/// </summary>
/// <param name="PreviousStopId">
/// The ID of the previous stop in the path. In forward traversal, this is the next stop to reach from the current one.
/// </param>
/// <param name="TripId">The ID of the trip used to reach this stop, or <see langword="null"/> if reached via transfer.</param>
/// <param name="ArrivalTime">The arrival time at the stop.</param>
/// <param name="Transfers">The number of transfers used to reach this stop.</param>
internal record BackPointer(
    string PreviousStopId,
    string? TripId,
    DateTime ArrivalTime,
    int Transfers);
