namespace Application.DTOs;

/// <summary>
/// Represents user's data used to log in.
/// </summary>
public class LoginUserDto
{
    /// <summary>
    /// Gets or sets user's email.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets user's password.
    /// </summary>
    public required string Password { get; set; }
}
