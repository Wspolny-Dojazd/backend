using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a section of a shape line, optionally associated with two stops.
/// </summary>
/// <param name="From">The optional starting stop unique identifier of the shape section.</param>
/// <param name="To">The optional ending stop unique identifier of the shape section.</param>
/// <param name="Coords">The list of coordinates that define the shape section.</param>
public record ShapeSectionDto(
    string? From,
    string? To,
    [property: Required] List<ShapeCoordDto> Coords);
