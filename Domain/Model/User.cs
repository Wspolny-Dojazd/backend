namespace Domain.Model;

/// <summary>
/// User model class that defines its properties.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets User Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets User's Email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets User's Nickname.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Gets or sets User's Password Hash.
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Gets or Sets the date of creating user's object.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets user's Friends.
    /// </summary>
    public List<User> Friends { get; set; }

    /// <summary>
    /// Gets or sets user's Groups.
    /// </summary>
    public List<Group> Groups { get; set; }

    /// <summary>
    /// Gets or sets User's Configuration.
    /// </summary>
    public UserConfiguration UserConfiguration { get; set; }
}
