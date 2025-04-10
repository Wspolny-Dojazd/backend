using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

/// <summary>
/// Represents the data transfer object used for user registration requests.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(32)]
    [RegularExpression("^[a-z0-9_]+$")]

    public required string Username { get; init; }

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

    /// <summary>
    /// Gets the nickname of the user.
    /// </summary>
    [Required]
    public required string Nickname { get; init; }
}
