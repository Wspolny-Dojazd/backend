namespace Domain.Model;

/// <summary>
/// Represents a chat message sent within a group.
/// </summary>
public class Message
{
    /// <summary>
    /// Gets or sets the unique identifier of the message.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the group where the message was sent.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who sent the message.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the message was sent.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the content of the message.
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// Gets or sets the group associated with the message.
    /// </summary>
    public Group Group { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user who sent the message.
    /// </summary>
    public User User { get; set; } = default!;
}
