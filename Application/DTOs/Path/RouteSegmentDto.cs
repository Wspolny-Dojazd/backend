using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a data transfer object used for route segments in a user's path.
/// </summary>
/// <param name="Line">The route line associated with the segment.</param>
/// <param name="Stops">The list of stops associated with the segment.</param>
/// <param name="Shapes">The list of shape sections representing the physical route of the segment.</param>
public record RouteSegmentDto(
    [property: Required] RouteLineDto Line,
    [property: Required] List<StopDto> Stops,
    [property: Required] List<ShapeSectionDto> Shapes) : SegmentDtoBase;
