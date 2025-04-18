using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a data transfer object used for an accepted group path.
/// </summary>
/// <param name="Id">The unique identifier of the accepted path.</param>
/// <param name="Paths">The user paths that make up the group path.</param>
public record GroupPathDto(
    [property: Required] Guid Id,
    [property: Required] IEnumerable<UserPathDto> Paths);
