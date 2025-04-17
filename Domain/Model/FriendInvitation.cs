namespace Domain.Model;

/// <summary>
/// Represents a friend invitation between two users in the system.
/// </summary>
public class FriendInvitation
{
    /// <summary>
    /// Gets or sets the unique identifier for the invitation.
    /// </summary>
    public Guid InvitationId { get; set; }

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
    /// Gets or sets the navigation property to the user who sent the invitation.
    /// This is used for eager loading the related sender information.
    /// </summary>
    public required User Sender { get; set; }

    /// <summary>
    /// Gets or sets the navigation property to the user who received the invitation.
    /// This is used for eager loading the related receiver information.
    /// </summary>
    public required User Receiver { get; set; }
}
