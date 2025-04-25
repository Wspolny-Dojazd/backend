namespace Domain.Model;

/// <summary>
/// Represents a friend invitation between two users in the system.
/// </summary>
public class FriendInvitation
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
}
