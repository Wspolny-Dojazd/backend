using Application.DTOs.FriendInvitation;

namespace Application.Interfaces;

/// <summary>
/// Service interface for managing friend invitations between users.
/// </summary>
public interface IFriendInvitationService
{
    /// <summary>
    /// Creates a new friend invitation from one user to another.
    /// </summary>
    /// <param name="senderId">The ID of the user sending the invitation.</param>
    /// <param name="dto">Data transfer object containing the recipient user ID.</param>
    /// <returns>A data transfer object representing the created invitation.</returns>
    /// <exception cref="Application.Exceptions.UserNotFoundException">Thrown when the recipient user doesn't exist.</exception>
    /// <exception cref="Application.Exceptions.CannotInviteSelfException">Thrown when a user attempts to invite themselves.</exception>
    /// <exception cref="Application.Exceptions.AlreadyFriendsException">Thrown when users are already friends.</exception>
    /// <exception cref="Application.Exceptions.FriendInvitationExistsException">Thrown when an invitation already exists between the users.</exception>
    Task<FriendInvitationDto> CreateInvitationAsync(Guid senderId, CreateFriendInvitationDto dto);

    /// <summary>
    /// Retrieves all invitations sent by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user who sent the invitations.</param>
    /// <returns>A list of data transfer objects representing the sent invitations.</returns>
    Task<List<FriendInvitationDto>> GetSentInvitationsAsync(Guid userId);

    /// <summary>
    /// Retrieves all invitations received by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user who received the invitations.</param>
    /// <returns>A list of data transfer objects representing the received invitations.</returns>
    Task<List<FriendInvitationDto>> GetReceivedInvitationsAsync(Guid userId);

    /// <summary>
    /// Accepts a friend invitation, creating a friendship between the users and removing the invitation.
    /// Also removes any reciprocal invitation if it exists.
    /// </summary>
    /// <param name="userId">The ID of the user accepting the invitation (must be the receiver).</param>
    /// <param name="invitationId">The ID of the invitation to accept.</param>
    /// <returns>A data transfer object representing the accepted invitation.</returns>
    /// <exception cref="Application.Exceptions.FriendInvitationNotFoundException">Thrown when the invitation doesn't exist.</exception>
    /// <exception cref="Application.Exceptions.UnauthorizedInvitationActionException">Thrown when the user is not authorized to accept the invitation.</exception>
    Task<FriendInvitationDto> AcceptInvitationAsync(Guid userId, Guid invitationId);

    /// <summary>
    /// Declines a friend invitation, removing it from the system.
    /// Also removes any reciprocal invitation if it exists.
    /// </summary>
    /// <param name="userId">The ID of the user declining the invitation (must be the receiver).</param>
    /// <param name="invitationId">The ID of the invitation to decline.</param>
    /// <returns>A data transfer object representing the declined invitation.</returns>
    /// <exception cref="Application.Exceptions.FriendInvitationNotFoundException">Thrown when the invitation doesn't exist.</exception>
    /// <exception cref="Application.Exceptions.UnauthorizedInvitationActionException">Thrown when the user is not authorized to decline the invitation.</exception>
    Task<FriendInvitationDto> DeclineInvitationAsync(Guid userId, Guid invitationId);

    /// <summary>
    /// Cancels a friend invitation that was previously sent.
    /// </summary>
    /// <param name="userId">The ID of the user canceling the invitation (must be the sender).</param>
    /// <param name="invitationId">The ID of the invitation to cancel.</param>
    /// <returns>A data transfer object representing the canceled invitation.</returns>
    /// <exception cref="Application.Exceptions.FriendInvitationNotFoundException">Thrown when the invitation doesn't exist.</exception>
    /// <exception cref="Application.Exceptions.UnauthorizedInvitationActionException">Thrown when the user is not authorized to cancel the invitation.</exception>
    Task<FriendInvitationDto> CancelInvitationAsync(Guid userId, Guid invitationId);
}
