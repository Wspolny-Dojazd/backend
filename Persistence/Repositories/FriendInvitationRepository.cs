using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Provides data access operations for <see cref="FriendInvitation"/> entities.
/// </summary>
/// <param name="databaseContext">The database context used to access friend invitation data.</param>
public class FriendInvitationRepository(DatabaseContext databaseContext)
    : IFriendInvitationRepository
{
    /// <inheritdoc/>
    public async Task AddAsync(FriendInvitation invitation)
    {
        _ = await databaseContext.FriendInvitations.AddAsync(invitation);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<FriendInvitation?> GetByIdAsync(Guid invitationId)
    {
        return await databaseContext.FriendInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .FirstOrDefaultAsync(i => i.Id == invitationId);
    }

    /// <inheritdoc/>
    public async Task<List<FriendInvitation>> GetSentAsync(Guid userId)
    {
        return await databaseContext.FriendInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Where(i => i.SenderId == userId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<FriendInvitation>> GetReceivedAsync(Guid userId)
    {
        return await databaseContext.FriendInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Where(i => i.ReceiverId == userId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(FriendInvitation invitation)
    {
        _ = databaseContext.FriendInvitations.Remove(invitation);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(Guid senderId, Guid receiverId)
    {
        return await databaseContext.FriendInvitations
            .AnyAsync(i => i.SenderId == senderId && i.ReceiverId == receiverId);
    }

    /// <inheritdoc/>
    public async Task<FriendInvitation?> GetByUsersAsync(Guid senderId, Guid receiverId)
    {
        return await databaseContext.FriendInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .FirstOrDefaultAsync(i => i.SenderId == senderId && i.ReceiverId == receiverId);
    }
}
