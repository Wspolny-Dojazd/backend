using Application.DTOs;
using Application.DTOs.GroupInvitation;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for managing group invitations.
/// </summary>
public interface IGroupInvitationService
{
    /// <summary>
    /// Sends a new group invitation from one user to another.
    /// </summary>
    /// <param name="senderId">The unique identifier of the user sending the invitation.</param>
    /// <param name="dto">The data transfer object containing the invitation details.</param>
    /// <returns>The created group invitation.</returns>
    Task<GroupInvitationDto> SendAsync(Guid senderId, GroupInvitationRequestDto dto);

    /// <summary>
    /// Retrieves all invitations sent by a specific user.
    /// </summary>
    /// <param name="senderId">The unique identifier of the user who sent the invitations.</param>
    /// <returns>Invitations sent by the specified user.</returns>
    Task<IEnumerable<GroupInvitationDto>> GetSentAsync(Guid senderId);

    /// <summary>
    /// Retrieves all invitations sent for a specific group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>Invitations sent for the specified group.</returns>
    Task<IEnumerable<GroupInvitationDto>> GetAllSentAsync(int groupId);

    /// <summary>
    /// Retrieves all invitations received by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who received the invitations.</param>
    /// <returns>Invitations received by the specified user.</returns>
    Task<IEnumerable<GroupInvitationDto>> GetReceivedAsync(Guid userId);

    /// <summary>
    /// Accepts a group invitation and adds the receiver to the group.
    /// </summary>
    /// <param name="userId">The unique identifier of the user accepting the invitation (must be the receiver).</param>
    /// <param name="invitationId">The unique identifier of the invitation to accept.</param>
    /// <returns>The group to which the user was added.</returns>
    /// <remarks>
    /// This method also removes any reciprocal invitation if it exists.
    /// </remarks>
    Task<GroupDto> AcceptAsync(Guid userId, Guid invitationId);

    /// <summary>
    /// Deletes a group invitation by the specified user.
    /// Executes a <c>cancel</c> operation if the user is the sender,
    /// or a <c>decline</c> operation if the user is the receiver.
    /// </summary>
    /// <param name="userId">The unique identifier of the user performing the deletion.</param>
    /// <param name="invitationId">The unique identifier of the invitation to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid userId, Guid invitationId);
}
