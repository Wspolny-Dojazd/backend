namespace Application.DTOs;

/// <summary>
/// Represents user's display data.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or sets user's identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets user's nickname.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Gets or sets user's email.
    /// </summary>
    public string Email { get; set; }
}
