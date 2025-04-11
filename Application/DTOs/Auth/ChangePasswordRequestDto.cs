using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

/// <summary>
/// Represents the data transfer object used for user registration requests.
/// </summary>
public class ChangePasswordRequestDto
{
    /// <summary>
    /// Gets the current password of the user.
    /// </summary>
    [Required]
    public required string CurrentPassword { get; init; }

    /// <summary>
    /// Gets the new password of the user.
    /// </summary>
    [Required]
    public required string NewPassword { get; init; }
}
