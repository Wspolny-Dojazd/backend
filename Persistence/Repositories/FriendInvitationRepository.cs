using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Repository implementation for performing data access operations on friend invitations.
/// </summary>
public class FriendInvitationRepository(DatabaseContext databaseContext) : IFriendInvitationRepository
{
    /// <summary>
    /// Creates a new friend invitation in the database.
    /// </summary>
    /// <param name="invitation">The friend invitation entity to create.</param>
    /// <returns>The created friend invitation with updated database-assigned values.</returns>
    public async Task<FriendInvitation> CreateAsync(FriendInvitation invitation)
    {
        await databaseContext.FriendInvitations.AddAsync(invitation);
        await databaseContext.SaveChangesAsync();
        return invitation;
    }

    /// <summary>
    /// Retrieves a friend invitation by its unique identifier.
    /// </summary>
    /// <param name="invitationId">The unique identifier of the invitation to retrieve.</param>
    /// <returns>The friend invitation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no invitation with the specified ID exists.</exception>
    public async Task<FriendInvitation> GetByIdAsync(Guid invitationId)
    {
        return await databaseContext.FriendInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .FirstAsync(i => i.InvitationId == invitationId);
    }

    /// <summary>
    /// Retrieves all invitations sent by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user who sent the invitations.</param>
    /// <returns>A list of friend invitations sent by the specified user.</returns>
    public async Task<List<FriendInvitation>> GetSentInvitationsAsync(Guid userId)
    {
        return await databaseContext.FriendInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Where(i => i.SenderId == userId)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all invitations received by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user who received the invitations.</param>
    /// <returns>A list of friend invitations received by the specified user.</returns>
    public async Task<List<FriendInvitation>> GetReceivedInvitationsAsync(Guid userId)
    {
        return await databaseContext.FriendInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Where(i => i.ReceiverId == userId)
            .ToListAsync();
    }

    /// <summary>
    /// Deletes a friend invitation from the database.
    /// </summary>
    /// <param name="invitationId">The unique identifier of the invitation to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    /// <remarks>
    /// If the invitation doesn't exist, this method completes without throwing an exception.
    /// </remarks>
    public async Task DeleteAsync(Guid invitationId)
    {
        var invitation = await databaseContext.FriendInvitations
            .FirstOrDefaultAsync(i => i.InvitationId == invitationId);

        if (invitation != null)
        {
            databaseContext.FriendInvitations.Remove(invitation);
            await databaseContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Checks if a friend invitation exists between the specified sender and receiver.
    /// </summary>
    /// <param name="senderId">The ID of the user who sent the invitation.</param>
    /// <param name="receiverId">The ID of the user who received the invitation.</param>
    /// <returns>True if an invitation exists; otherwise, false.</returns>
    public async Task<bool> ExistsAsync(Guid senderId, Guid receiverId)
    {
        return await databaseContext.FriendInvitations
            .AnyAsync(i => i.SenderId == senderId && i.ReceiverId == receiverId);
    }
}
