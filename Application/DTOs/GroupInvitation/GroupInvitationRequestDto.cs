using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.GroupInvitation;

/// <summary>
/// Represents a data transfer object for sending a group invitation.
/// </summary>
public class GroupInvitationRequestDto
{
    /// <summary>
    /// Gets the unique identifier of the user to whom the invitation is sent.
    /// </summary>
    [Required]
    public Guid UserId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the group to which the invitation is sent.
    /// </summary>
    [Required]
    public int GroupId { get; init; }
}
