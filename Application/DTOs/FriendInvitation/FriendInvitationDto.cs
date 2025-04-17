namespace Application.DTOs.FriendInvitation;

/// <summary>
/// Data transfer object for friend invitations.
/// Contains invitation details and related user information for API responses.
/// </summary>
public class FriendInvitationDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the invitation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the invitation was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets information about the user who sent the invitation.
    /// </summary>
    public required UserDto Sender { get; set; }

    /// <summary>
    /// Gets or sets information about the user who received the invitation.
    /// </summary>
    public required UserDto Receiver { get; set; }
}
