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
    public LocationType LocationType { get; init; }

    /// <summary>
    /// Gets the parent station ID, if the stop belongs to a station.
    /// </summary>
    public string? ParentStationId { get; init; }

    /// <summary>
    /// Gets a value indicating whether the stop is wheelchair accessible.
    /// </summary>
    public bool WheelchairBoarding { get; init; }

    /// <summary>
    /// Gets the normalized or stemmed version of the stop name.
    /// </summary>
    public string? NameStem { get; init; }

    /// <summary>
    /// Gets the town where the stop is located.
    /// </summary>
    public string? TownName { get; init; }

    /// <summary>
    /// Gets the street where the stop is located.
    /// </summary>
    public string? StreetName { get; init; }
}
