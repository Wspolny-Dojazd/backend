namespace Domain.Model;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the nickname of the user.
    /// </summary>
    public required string Nickname { get; set; }

    /// <summary>
    /// Gets or sets the password hash of the user.
    /// </summary>
    public required string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the friends of the user.
    /// </summary>
    public required List<User> Friends { get; set; }

    /// <summary>
    /// Gets or sets the groups to which the user belongs.
    /// </summary>
    public required List<Group> Groups { get; set; }

    /// <summary>
    /// Gets or sets the configuration of the user.
    /// </summary>
    public required UserConfiguration UserConfiguration { get; set; }
}
