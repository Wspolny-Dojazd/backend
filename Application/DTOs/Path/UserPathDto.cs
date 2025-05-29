using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a data transfer object for a user's entire travel path.
/// </summary>
/// <param name="UserId">The unique identifier of the user.</param>
/// <param name="DepartureTime">The departure time.</param>
/// <param name="Segments">The list of segments in the user's path.</param>
public record UserPathDto(
    [property: Required] Guid UserId,
    [property: Required] DateTime DepartureTime,
    [property: Required] List<SegmentDtoBase> Segments);
