namespace Application.DTOs;

public class RegisterUserDto
{
    /// <summary>
    /// Gets or sets user's Email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets user's Nickname.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Gets or sets user's password.
    /// </summary>
    public string Password { get; set; }
}