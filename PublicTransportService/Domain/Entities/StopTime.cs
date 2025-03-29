using PublicTransportService.Domain.Enums;

namespace PublicTransportService.Domain.Entities;

/// <summary>
/// Represents a stop time on a trip.
/// </summary>
public class StopTime
{
    /// <summary>
    /// Gets the unique identifier of the stop time.
    /// </summary>
    /// <remarks>
    /// This value is not provided by the GTFS feed.
    /// </remarks>
    public int Id { get; init; }

    /// <summary>
    /// Gets the identifier of the trip to which this stop time belongs.
    /// </summary>
    public required string TripId { get; init; }

    /// <summary>
    /// Gets the order of the stop within the trip.
    /// </summary>
    public int StopSequence { get; init; }

    /// <summary>
    /// Gets the identifier of the stop associated with this stop time.
    /// </summary>
    public required string StopId { get; init; }

    /// <summary>
    /// Gets the scheduled arrival time at the stop.
    /// </summary>
    public DateTime ArrivalTime { get; init; }

    /// <summary>
    /// Gets the scheduled departure time from the stop.
    /// </summary>
    public DateTime DepartureTime { get; init; }

    /// <summary>
    /// Gets the pickup type at the stop.
    /// </summary>
    public PickupType PickupType { get; init; }

    /// <summary>
    /// Gets the drop-off type at the stop.
    /// </summary>
    public DropOffType DropOffType { get; init; }
}
