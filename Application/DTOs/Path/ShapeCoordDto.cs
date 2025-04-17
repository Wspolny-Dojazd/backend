using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a shape coordinate used to describe a part of a route or path.
/// </summary>
/// <param name="Latitude">The latitude of the shape coordinate.</param>
/// <param name="Longitude">The longitude of the shape coordinate.</param>
public record ShapeCoordDto(
    [property: Required] double Latitude,
    [property: Required] double Longitude);
