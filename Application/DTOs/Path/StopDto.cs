using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a data transfer object used for stops.
/// </summary>
/// <param name="Id">The unique identifier of the stop.</param>
/// <param name="Name">The name of the stop.</param>
/// <param name="Code">The code of the stop, if applicable.</param>
/// <param name="Latitude">The latitude coordinate of the stop.</param>
/// <param name="Longitude">The longitude coordinate of the stop.</param>
/// <param name="WheelchairAccessible">Indicates whether the stop is wheelchair accessible.</param>
/// <param name="ArrivalTime">The scheduled arrival time, or <see langword="null"/> if the stop is first in the leg.</param>
/// <param name="DepartureTime">The scheduled departure time, or <see langword="null"/> if the stop is last in the leg.</param>
public record StopDto(
    [property: Required] string Id,
    [property: Required] string Name,
    [property: Required] string? Code,
    [property: Required] double Latitude,
    [property: Required] double Longitude,
    [property: Required] bool WheelchairAccessible,
    DateTime? ArrivalTime,
    DateTime? DepartureTime);
