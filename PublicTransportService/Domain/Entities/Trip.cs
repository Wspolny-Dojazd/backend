using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportService.Domain.Entities;

/// <summary>
/// Represents a scheduled public transport trip.
/// </summary>
public class Trip
{
    /// <summary>
    /// Gets the unique identifier of the trip.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the identifier of the route to which the trip belongs.
    /// </summary>
    [Column("route_id")]
    public required string RouteId { get; init; }

    /// <summary>
    /// Gets the identifier of the service calendar applicable to the trip.
    /// </summary>
    [Column("service_id")]
    public required string ServiceId { get; init; }

    /// <summary>
    /// Gets the identifier of the shape that represents the trip's path.
    /// </summary>
    [Column("shape_id")]
    public required string ShapeId { get; init; }

    /// <summary>
    /// Gets the short name of the trip.
    /// </summary>
    [Column("trip_short_name")]
    public required string ShortName { get; init; }

    /// <summary>
    /// Gets the headsign or destination displayed on the vehicle.
    /// </summary>
    [Column("head_sign")]
    public required string HeadSign { get; init; }

    /// <summary>
    /// Gets the direction of travel for the trip.
    /// </summary>
    /// <remarks>
    /// The direction is typically represented as an integer,
    /// where 0 indicates the outbound direction and 1 indicates the inbound direction.
    /// </remarks>
    [Column("direction_id")]
    public int DirectionId { get; init; }

    /// <summary>
    /// Gets a value indicating whether the trip is wheelchair accessible.
    /// </summary>
    [Column("wheelchair_accessible")]
    public bool WheelchairAccessible { get; init; }

    /// <summary>
    /// Gets the hidden block identifier for grouping trips operationally.
    /// </summary>
    [Column("hidden_block_id")]
    public string? HiddenBlockId { get; init; }

    /// <summary>
    /// Gets the brigade code assigned to the vehicle or driver, if applicable.
    /// </summary>
    [Column("brigade")]
    public string? Brigade { get; init; }

    /// <summary>
    /// Gets the type of vehicle fleet assigned to the trip, if specified.
    /// </summary>
    [Column("fleet_type")]
    public string? FleetType { get; init; }
}
