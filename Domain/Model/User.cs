namespace Domain.Model;

/// <summary>
/// Represents an user.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the user's unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user's Email.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the user's Nickname.
    /// </summary>
    public required string Nickname { get; set; }

    /// <summary>
    /// Gets or sets the user's password hash.
    /// </summary>
    public required string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the date of creating user's object.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user's friend list.
    /// </summary>
    public required List<User> Friends { get; set; }

    /// <summary>
    /// Gets or sets the user's groups.
    /// </summary>
    public required List<Group> Groups { get; set; }

    /// <summary>
    /// Gets or sets the user's configuration.
    /// </summary>
    public required UserConfiguration UserConfiguration { get; set; }
}
