using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Repository interface for performing data access operations on friend invitations.
/// </summary>
public interface IFriendInvitationRepository
{
    /// <summary>
    /// Creates a new friend invitation in the database.
    /// </summary>
    /// <param name="invitation">The friend invitation entity to create.</param>
    /// <returns>The created friend invitation with generated ID and other database-assigned values.</returns>
    Task<FriendInvitation> CreateAsync(FriendInvitation invitation);

    /// <summary>
    /// Retrieves a friend invitation by its unique identifier.
    /// </summary>
    /// <param name="invitationId">The unique identifier of the invitation to retrieve.</param>
    /// <returns>The friend invitation if found; otherwise, null.</returns>
    Task<FriendInvitation> GetByIdAsync(Guid invitationId);

    /// <summary>
    /// Retrieves all invitations sent by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user who sent the invitations.</param>
    /// <returns>A list of friend invitations sent by the specified user.</returns>
    Task<List<FriendInvitation>> GetSentInvitationsAsync(Guid userId);

    /// <summary>
    /// Retrieves all invitations received by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user who received the invitations.</param>
    /// <returns>A list of friend invitations received by the specified user.</returns>
    Task<List<FriendInvitation>> GetReceivedInvitationsAsync(Guid userId);

    /// <summary>
    /// Deletes a friend invitation from the database.
    /// </summary>
    /// <param name="invitationId">The unique identifier of the invitation to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid invitationId);

    /// <summary>
    /// Checks if a friend invitation exists between the specified sender and receiver.
    /// </summary>
    /// <param name="senderId">The ID of the user who sent the invitation.</param>
    /// <param name="receiverId">The ID of the user who received the invitation.</param>
    /// <returns>True if an invitation exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(Guid senderId, Guid receiverId);
}
