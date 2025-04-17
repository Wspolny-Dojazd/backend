using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a data transfer object used for proposed path.
/// </summary>
/// <param name="Id">The unique identifier of the proposed path.</param>
/// <param name="Paths">The list of user paths associated with the proposed path.</param>
public record ProposedPathDto(
    [property: Required] Guid Id,
    [property: Required] IEnumerable<UserPathDto> Paths);
