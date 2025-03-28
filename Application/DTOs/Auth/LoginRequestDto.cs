using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

/// <summary>
/// Represents the data transfer object used for user login requests.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    /// <summary>
    /// Gets the password of the user.
    /// </summary>
    [Required]
    public required string Password { get; init; }
}
