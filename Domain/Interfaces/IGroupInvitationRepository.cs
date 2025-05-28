using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="GroupInvitation"/> data access operations.
/// </summary>
public interface IGroupInvitationRepository
{
    /// <summary>
    /// Adds a new group invitation to the database.
    /// </summary>
    /// <param name="invitation">The group invitation entity to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(GroupInvitation invitation);

    /// <summary>
    /// Retrieves a group invitation by its unique identifier.
    /// </summary>
    /// <param name="invitationId">The unique identifier of the invitation to retrieve.</param>
    /// <returns>The group invitation if found; otherwise, <see langword="null"/>.</returns>
    Task<GroupInvitation?> GetByIdAsync(Guid invitationId);

    /// <summary>
    /// Retrieves all invitations sent by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who sent the invitations.</param>
    /// <returns>The group invitations sent by the specified user.</returns>
    Task<List<GroupInvitation>> GetSentAsync(Guid userId);

    /// <summary>
    /// Retrieves all invitations received by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who received the invitations.</param>
    /// <returns>The group invitations received by the specified user.</returns>
    Task<List<GroupInvitation>> GetReceivedAsync(Guid userId);

    /// <summary>
    /// Retrieves all invitations to the specific group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>The group invitations targeting the specified group.</returns>
    Task<List<GroupInvitation>> GetAllAsync(int groupId);

    /// <summary>
    /// Deletes a group invitation from the database.
    /// </summary>
    /// <param name="invitation">The group invitation entity to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(GroupInvitation invitation);

    /// <summary>
    /// Checks whether a group invitation exists between the specified sender and receiver.
    /// </summary>
    /// <param name="senderId">The unique identifier of the user who sent the invitation.</param>
    /// <param name="receiverId">The unique identifier of the user who received the invitation.</param>
    /// <param name="groupId"> The unique identifier of the group to which the invitation pertains.</param>
    /// <returns><see langword="true"/> if an invitation exists; otherwise, <see langword="false"/>.</returns>
    Task<bool> ExistsAsync(Guid senderId, Guid receiverId, int groupId);

    /// <summary>
    /// Retrieves a group invitation by the sender and receiver unique identifiers.
    /// </summary>
    /// <param name="senderId">The unique identifier of the user who sent the invitation.</param>
    /// <param name="receiverId">The unique identifier of the user who received the invitation.</param>
    /// <param name="groupId"> The unique identifier of the group to which the invitation pertains.</param>
    /// <returns>The group invitation if found; otherwise, <see langword="null"/>.</returns>
    Task<GroupInvitation?> GetByUsersAsync(Guid senderId, Guid receiverId, int groupId);
}
