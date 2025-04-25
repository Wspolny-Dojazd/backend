using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.FriendInvitation;

/// <summary>
/// Represents a data transfer object for sending a friend invitation.
/// </summary>
public class FriendInvitationRequestDto
{
    /// <summary>
    /// Gets the unique identifier of the user to whom the invitation is sent.
    /// </summary>
    [Required]
    public Guid UserId { get; init; }
}
