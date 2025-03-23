namespace Application.DTOs;

/// <summary>
/// Represents user's data used during registration.
/// </summary>
public class RegisterUserDto
{
    /// <summary>
    /// Gets or sets user's Email.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets user's Nickname.
    /// </summary>
    public required string Nickname { get; set; }

    /// <summary>
    /// Gets or sets user's password.
    /// </summary>
    public required string Password { get; set; }
}
