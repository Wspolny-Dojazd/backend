using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

/// <summary>
/// Represents the data transfer object used for friend invitation responses.
/// </summary>
/// <param name="Id">The unique identifier of the invitation.</param>
/// <param name="CreatedAt">The date and time when the invitation was created.</param>
/// <param name="Sender">The user who sent the invitation.</param>
/// <param name="Receiver">The user who received the invitation.</param>
public record FriendInvitationDto(
    [property: Required] Guid Id,
    [property: Required] DateTime CreatedAt,
    [property: Required] UserDto Sender,
    [property: Required] UserDto Receiver);
