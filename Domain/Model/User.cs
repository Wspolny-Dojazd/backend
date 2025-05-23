namespace Domain.Model;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public required string Username { get; set; }

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
    /// Gets or sets the refresh token used for token renewal.
    /// </summary>
    public required string RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the expiration date and time of the current refresh token.
    /// </summary>
    public DateTime RefreshTokenExpiryTime { get; set; }

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

    /// <summary>
    /// Gets or sets the location of the user.
    /// </summary>
    public UserLocation? UserLocation { get; set; }
}
