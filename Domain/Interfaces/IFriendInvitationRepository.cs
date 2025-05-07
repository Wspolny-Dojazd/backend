using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="FriendInvitation"/> data access operations.
/// </summary>
public interface IFriendInvitationRepository
{
    /// <summary>
    /// Adds a new friend invitation to the database.
    /// </summary>
    /// <param name="invitation">The friend invitation entity to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(FriendInvitation invitation);

    /// <summary>
    /// Retrieves a friend invitation by its unique identifier.
    /// </summary>
    /// <param name="invitationId">The unique identifier of the invitation to retrieve.</param>
    /// <returns>The friend invitation if found; otherwise, <see langword="null"/>.</returns>
    Task<FriendInvitation?> GetByIdAsync(Guid invitationId);

    /// <summary>
    /// Retrieves all invitations sent by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who sent the invitations.</param>
    /// <returns>The friend invitations sent by the specified user.</returns>
    Task<List<FriendInvitation>> GetSentAsync(Guid userId);

    /// <summary>
    /// Retrieves all invitations received by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who received the invitations.</param>
    /// <returns>The friend invitations received by the specified user.</returns>
    Task<List<FriendInvitation>> GetReceivedAsync(Guid userId);

    /// <summary>
    /// Deletes a friend invitation from the database.
    /// </summary>
    /// <param name="invitation">The friend invitation entity to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(FriendInvitation invitation);

    /// <summary>
    /// Checks whether a friend invitation exists between the specified sender and receiver.
    /// </summary>
    /// <param name="senderId">The unique identifier of the user who sent the invitation.</param>
    /// <param name="receiverId">The unique identifier of the user who received the invitation.</param>
    /// <returns><see langword="true"/> if an invitation exists; otherwise, <see langword="false"/>.</returns>
    Task<bool> ExistsAsync(Guid senderId, Guid receiverId);

    /// <summary>
    /// Retrieves a friend invitation by the sender and receiver unique identifiers.
    /// </summary>
    /// <param name="senderId">The unique identifier of the user who sent the invitation.</param>
    /// <param name="receiverId">The unique identifier of the user who received the invitation.</param>
    /// <returns>The friend invitation if found; otherwise, <see langword="null"/>.</returns>
    Task<FriendInvitation?> GetByUsersAsync(Guid senderId, Guid receiverId);
}
