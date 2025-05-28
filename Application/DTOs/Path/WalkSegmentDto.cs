using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a data transfer object for a walking segment between two locations.
/// </summary>
/// <param name="From">The origin walk location.</param>
/// <param name="To">The destination walk location.</param>
/// <param name="Duration">The walking time in minutes.</param>
/// <param name="Distance">The distance in meters.</param>
/// <param name="Shapes">The list of shape sections representing the walk path.</param>
public record WalkSegmentDto(
    [property: Required] WalkLocationDto From,
    [property: Required] WalkLocationDto To,
    [property: Required] int Duration,
    [property: Required] int Distance,
    [property: Required] List<ShapeSectionDto> Shapes) : SegmentDtoBase;
