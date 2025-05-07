using System.ComponentModel.DataAnnotations.Schema;
using PublicTransportService.Domain.Enums;

namespace PublicTransportService.Domain.Entities;

/// <summary>
/// Represents a public transport stop or station.
/// </summary>
public class Stop
{
    /// <summary>
    /// Gets the unique identifier of the stop.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the name of the stop.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the code of the stop.
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    /// Gets the platform code of the stop, if any.
    /// </summary>
    [Column("platform_code")]
    public string? PlatformCode { get; init; }

    /// <summary>
    /// Gets the latitude of the stop location.
    /// </summary>
    public double Latitude { get; init; }

    /// <summary>
    /// Gets the longitude of the stop location.
    /// </summary>
    public double Longitude { get; init; }

    /// <summary>
    /// Gets the location type of the stop.
    /// </summary>
    [Column("location_type")]
    public LocationType LocationType { get; init; }

    /// <summary>
    /// Gets the parent station ID, if the stop belongs to a station.
    /// </summary>
    [Column("parent_station")]
    public string? ParentStation { get; init; }

    /// <summary>
    /// Gets a value indicating whether the stop is wheelchair accessible.
    /// </summary>
    [Column("wheelchair_boarding")]
    public bool WheelchairBoarding { get; init; }

    /// <summary>
    /// Gets the normalized or stemmed version of the stop name.
    /// </summary>
    [Column("name_stem")]
    public string? NameStem { get; init; }

    /// <summary>
    /// Gets the town where the stop is located.
    /// </summary>
    [Column("town_name")]
    public string? TownName { get; init; }

    /// <summary>
    /// Gets the street where the stop is located.
    /// </summary>
    [Column("street_name")]
    public string? StreetName { get; init; }

    /// <summary>
    /// Gets the logical identifier of the stop, derived from the first four characters of its ID.
    /// Used to group physical stop variants that represent the same real-world location.
    /// </summary>
    /// <remarks>
    /// This is a heuristic for grouping stops before proper support for complex hierarchies (e.g., metro stations).
    /// </remarks>
    [NotMapped]
    public string LogicalId => this.Id[..4];
}
