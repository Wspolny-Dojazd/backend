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
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the content of the message.
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// Gets or sets the group associated with the message.
    /// </summary>
    public required Group Group { get; set; }

    /// <summary>
    /// Gets or sets the user who sent the message.
    /// </summary>
    public required User User { get; set; }
}
