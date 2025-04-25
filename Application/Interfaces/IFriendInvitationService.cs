using Application.DTOs.FriendInvitation;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for managing friend invitations.
/// </summary>
public interface IFriendInvitationService
{
    /// <summary>
    /// Sends a new friend invitation from one user to another.
    /// </summary>
    /// <param name="senderId">The unique identifier of the user sending the invitation.</param>
    /// <param name="dto">The data transfer object containing the invitation details.</param>
    /// <returns>The created friend invitation.</returns>
    Task<FriendInvitationDto> SendAsync(Guid senderId, FriendInvitationRequestDto dto);

    /// <summary>
    /// Retrieves all invitations sent by a specific user.
    /// </summary>
    /// <param name="senderId">The unique identifier of the user who sent the invitations.</param>
    /// <returns>Invitations sent by the specified user.</returns>
    Task<IEnumerable<FriendInvitationDto>> GetSentAsync(Guid senderId);

    /// <summary>
    /// Retrieves all invitations received by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who received the invitations.</param>
    /// <returns>Invitations received by the specified user.</returns>
    Task<IEnumerable<FriendInvitationDto>> GetReceivedAsync(Guid userId);

    /// <summary>
    /// Accepts a friend invitation and creates a friendship between the sender and receiver.
    /// </summary>
    /// <param name="userId">The unique identifier of the user accepting the invitation (must be the receiver).</param>
    /// <param name="invitationId">The unique identifier of the invitation to accept.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method also removes any reciprocal invitation if it exists.
    /// </remarks>
    Task AcceptAsync(Guid userId, Guid invitationId);

    /// <summary>
    /// Deletes a friend invitation by the specified user.
    /// Executes a <c>cancel</c> operation if the user is the sender,
    /// or a <c>decline</c> operation if the user is the receiver.
    /// </summary>
    /// <param name="userId">The unique identifier of the user performing the deletion.</param>
    /// <param name="invitationId">The unique identifier of the invitation to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid userId, Guid invitationId);
}
