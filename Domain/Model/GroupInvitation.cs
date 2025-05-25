namespace Domain.Model;

/// <summary>
/// Represents a group invitation between a user and a group in the system.
/// </summary>
public class GroupInvitation
{
    /// <summary>
    /// Gets or sets the unique identifier for the invitation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who sent the invitation.
    /// </summary>
    public Guid SenderId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who received the invitation.
    /// </summary>
    public Guid ReceiverId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the group to which the invitation is related.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the invitation was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user who sent the invitation.
    /// </summary>
    public User Sender { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user who received the invitation.
    /// </summary>
    public User Receiver { get; set; } = default!;

    /// <summary>
    /// Gets or sets the group to which the invitation is related.
    /// </summary>
    public Group Group { get; set; } = default!;
}
