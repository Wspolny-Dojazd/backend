using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportService.Domain.Entities;

/// <summary>
/// Represents a frequency of a public transport route.
/// </summary>
public class Frequency
{
    /// <summary>
    /// Gets the unique identifier of the route.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the unique identifier of the trip associated with this frequency.
    /// </summary>
    [Column("trip_id")]
    public required string TripId { get; init; }

    /// <summary>
    /// Gets the start time of the frequency period.
    /// </summary>
    [Column("start_time")]
    public int StartTime { get; init; }

    /// <summary>
    /// Gets the end time of the frequency period.
    /// </summary>
    [Column("end_time")]
    public int EndTime { get; init; }

    /// <summary>
    /// Gets the headway time in seconds.
    /// </summary>
    [Column("headway_secs")]
    public int Headway { get; init; }

    /// <summary>
    /// Gets the exact times for the frequency.
    /// </summary>
    [Column("exact_times")]
    public int ExactTimes { get; init; }
}
