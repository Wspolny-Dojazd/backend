using System.ComponentModel.DataAnnotations.Schema;
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
    [Column("trip_id")]
    public required string TripId { get; init; }

    /// <summary>
    /// Gets the order of the stop within the trip.
    /// </summary>
    [Column("stop_sequence")]
    public int StopSequence { get; init; }

    /// <summary>
    /// Gets the identifier of the stop associated with this stop time.
    /// </summary>
    [Column("stop_id")]
    public required string StopId { get; init; }

    /// <summary>
    /// Gets the scheduled arrival time at the stop, in seconds since midnight.
    /// </summary>
    [Column("arrival_time")]
    public int ArrivalTime { get; init; }

    /// <summary>
    /// Gets the scheduled departure time from the stop, in seconds since midnight.
    /// </summary>
    [Column("departure_time")]
    public int DepartureTime { get; init; }

    /// <summary>
    /// Gets the pickup type at the stop.
    /// </summary>
    [Column("pickup_type")]
    public PickupType PickupType { get; init; }

    /// <summary>
    /// Gets the drop-off type at the stop.
    /// </summary>
    [Column("drop_off_type")]
    public DropOffType DropOffType { get; init; }
}
