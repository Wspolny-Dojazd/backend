using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a location used in a walking segment.
/// </summary>
/// <param name="Id">The unique identifier of the walk location.</param>
/// <param name="Latitude">The latitude of the walk location.</param>
/// <param name="Longitude">The longitude of the walk location.</param>
/// <param name="Name">The name of the walk location.</param>
public record WalkLocationDto(
    [property: Required] string Id,
    [property: Required] double Latitude,
    [property: Required] double Longitude,
    string? Name);
