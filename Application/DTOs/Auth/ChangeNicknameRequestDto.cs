using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

/// <summary>
/// Represents the data transfer object used for nickname change requests.
/// </summary>
public class ChangeNicknameRequestDto
{
    /// <summary>
    /// Gets the new nickname of the user.
    /// </summary>
    [Required]
    public required string NewNickname { get; init; }
}
