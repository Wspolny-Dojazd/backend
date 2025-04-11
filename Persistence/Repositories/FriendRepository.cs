using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FriendRepository(DatabaseContext databaseContext) : IFriendRepository
{
    public async Task<FriendInvitation> CreateInvitationAsync(FriendInvitation invitation)
    {
        await databaseContext.FriendInvitations.AddAsync(invitation);
        await databaseContext.SaveChangesAsync();
        return invitation;
    }

    public async Task<List<FriendInvitation>> GetSentInvitationsAsync(Guid userId)
    {
        return await databaseContext.FriendInvitations
            .Include(fi => fi.Sender)
            .Include(fi => fi.Receiver)
            .Where(fi => fi.SenderId == userId)
            .ToListAsync();
    }

    public async Task<List<FriendInvitation>> GetReceivedInvitationsAsync(Guid userId)
    {
        return await databaseContext.FriendInvitations
            .Include(fi => fi.Sender)
            .Include(fi => fi.Receiver)
            .Where(fi => fi.ReceiverId == userId)
            .ToListAsync();
    }

    public async Task<FriendInvitation> GetInvitationByIdAsync(Guid invitationId)
    {
        return await databaseContext.FriendInvitations
            .Include(fi => fi.Sender)
            .Include(fi => fi.Receiver)
            .FirstOrDefaultAsync(fi => fi.InvitationId == invitationId);
    }

    public async Task<bool> InvitationExistsAsync(Guid senderId, Guid receiverId)
    {
        return await databaseContext.FriendInvitations
            .AnyAsync(fi => (fi.SenderId == senderId && fi.ReceiverId == receiverId) ||
                            (fi.SenderId == receiverId && fi.ReceiverId == senderId));
    }

    public async Task DeleteInvitationAsync(FriendInvitation invitation)
    {
        databaseContext.FriendInvitations.Remove(invitation);
        await databaseContext.SaveChangesAsync();
    }

    public async Task<bool> AreFriendsAsync(Guid userId1, Guid userId2)
    {
        return await databaseContext.Friends
            .AnyAsync(f => (f.UserId == userId1 && f.FriendId == userId2) ||
                           (f.UserId == userId2 && f.FriendId == userId1));
    }

    public async Task AddFriendAsync(Guid userId1, Guid userId2)
    {
        var friendship1 = new Friend
        {
            UserId = userId1,
            FriendId = userId2,
        };

        var friendship2 = new Friend
        {
            UserId = userId2,
            FriendId = userId1,
        };

        await databaseContext.Friends.AddAsync(friendship1);
        await databaseContext.Friends.AddAsync(friendship2);
        await databaseContext.SaveChangesAsync();
    }
}
