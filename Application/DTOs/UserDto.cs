namespace Application.DTOs;

/// <summary>
/// Represents user's display data.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or sets user's identifier.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Gets or sets user's nickname.
    /// </summary>
    public required string Nickname { get; set; }

    /// <summary>
    /// Gets or sets user's email.
    /// </summary>
    public required string Email { get; set; }
}
