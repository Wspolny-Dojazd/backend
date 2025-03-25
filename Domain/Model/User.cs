namespace Domain.Model;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the user's unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the user's nickname.
    /// </summary>
    public required string Nickname { get; set; }

    /// <summary>
    /// Gets or sets the hash of the user's password.
    /// </summary>
    public required string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the list of user's friends.
    /// </summary>
    public required List<User> Friends { get; set; }

    /// <summary>
    /// Gets or sets the list of groups the user belongs to.
    /// </summary>
    public required List<Group> Groups { get; set; }

    /// <summary>
    /// Gets or sets the user's configuration preferences.
    /// </summary>
    public required UserConfiguration UserConfiguration { get; set; }
}
