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
    /// Gets or sets user's Email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets user's Nickname.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Gets or sets user's password hash.
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the date of creating user's object.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets user's friend list.
    /// </summary>
    public List<User> Friends { get; set; }

    /// <summary>
    /// Gets or sets user's groups.
    /// </summary>
    public List<Group> Groups { get; set; }

    /// <summary>
    /// Gets or sets user's configuration.
    /// </summary>
    public UserConfiguration UserConfiguration { get; set; }
}
