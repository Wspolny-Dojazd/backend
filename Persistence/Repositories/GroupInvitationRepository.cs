using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Provides data access operations for <see cref="GroupInvitation"/> entities.
/// </summary>
/// <param name="databaseContext">The database context used to access group invitation data.</param>
public class GroupInvitationRepository(DatabaseContext databaseContext)
    : IGroupInvitationRepository
{
    /// <inheritdoc/>
    public async Task AddAsync(GroupInvitation invitation)
    {
        _ = await databaseContext.GroupInvitations.AddAsync(invitation);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<GroupInvitation?> GetByIdAsync(Guid invitationId)
    {
        return await databaseContext.GroupInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Include(i => i.Group)
            .FirstOrDefaultAsync(i => i.Id == invitationId);
    }

    /// <inheritdoc/>
    public async Task<List<GroupInvitation>> GetSentAsync(Guid userId)
    {
        return await databaseContext.GroupInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Include(i => i.Group)
            .Where(i => i.SenderId == userId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<GroupInvitation>> GetReceivedAsync(Guid userId)
    {
        return await databaseContext.GroupInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Include(i => i.Group)
            .Where(i => i.ReceiverId == userId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<GroupInvitation>> GetAllAsync(int groupId)
    {
        return await databaseContext.GroupInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Include(i => i.Group)
            .Where(i => i.GroupId == groupId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(GroupInvitation invitation)
    {
        _ = databaseContext.GroupInvitations.Remove(invitation);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(Guid senderId, Guid receiverId, int groupId)
    {
        return await databaseContext.GroupInvitations
            .AnyAsync(i => i.SenderId == senderId && i.ReceiverId == receiverId && i.GroupId == groupId);
    }

    /// <inheritdoc/>
    public async Task<GroupInvitation?> GetByUsersAsync(Guid senderId, Guid receiverId, int groupId)
    {
        return await databaseContext.GroupInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Include(i => i.Group)
            .FirstOrDefaultAsync(i => i.SenderId == senderId && i.ReceiverId == receiverId && i.GroupId == groupId);
    }
}
