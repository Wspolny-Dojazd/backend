namespace Domain.Model;

/// <summary>
/// Represents a friendship between two users.
/// </summary>
public class Friend
{
    /// <summary>
    /// Gets or sets the ID of the user who is part of this friendship.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the friend of the user.
    /// </summary>
    public Guid FriendId { get; set; }

    /// <summary>
    /// Gets or sets the user who is part of this friendship.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Gets or sets the friend of the user.
    /// </summary>
    public User FriendUser { get; set; }
}
