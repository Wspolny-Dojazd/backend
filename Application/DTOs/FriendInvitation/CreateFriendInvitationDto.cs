namespace Application.DTOs.FriendInvitation;

/// <summary>
/// Data transfer object for creating a new friend invitation.
/// Contains the necessary information to send an invitation to another user.
/// </summary>
public class CreateFriendInvitationDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the user who will receive the invitation.
    /// This is the target user that the authenticated user wants to become friends with.
    /// </summary>
    public Guid UserId { get; set; }
}
