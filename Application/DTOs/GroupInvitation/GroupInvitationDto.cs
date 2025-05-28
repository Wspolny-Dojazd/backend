using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.GroupInvitation;

/// <summary>
/// Represents a data transfer object for a group invitation.
/// </summary>
/// <param name="Id">The unique identifier for the invitation.</param>
/// <param name="CreatedAt">The date and time when the invitation was created.</param>
/// <param name="Sender">Information about the user who sent the invitation.</param>
/// <param name="Receiver">Information about the user who received the invitation.</param>
/// <param name="Group">Information about the group to which the invitation is sent.</param>
public record class GroupInvitationDto(
    [property: Required] Guid Id,
    [property: Required] DateTime CreatedAt,
    [property: Required] UserDto Sender,
    [property: Required] UserDto Receiver,
    [property: Required] GroupDto Group);
