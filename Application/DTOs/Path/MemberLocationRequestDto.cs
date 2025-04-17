using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserLocation;

/// <summary>
/// Represents a data transfer object used for location of a group member.
/// </summary>
public class MemberLocationRequestDto : UserLocationRequestDto
{
    /// <summary>
    /// Gets the unique identifier of the group member.
    /// </summary>
    [Required]
    public Guid UserId { get; init; }
}
