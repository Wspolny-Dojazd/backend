using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FriendInvitationRepository : IFriendInvitationRepository
{
    private readonly DatabaseContext _context;

    public FriendInvitationRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<FriendInvitation> CreateAsync(FriendInvitation invitation)
    {
        await _context.FriendInvitations.AddAsync(invitation);
        await _context.SaveChangesAsync();
        return invitation;
    }

    public async Task<FriendInvitation> GetByIdAsync(Guid invitationId)
    {
        return await _context.FriendInvitations.Include(i => i.Sender).Include(i => i.Receiver).FirstOrDefaultAsync(i => i.InvitationId == invitationId);
    }

    public async Task<List<FriendInvitation>> GetSentInvitationsAsync(Guid userId)
    {
        return await _context.FriendInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Where(i => i.SenderId == userId)
            .ToListAsync();
    }

    public async Task<List<FriendInvitation>> GetReceivedInvitationsAsync(Guid userId)
    {
        return await _context.FriendInvitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Where(i => i.ReceiverId == userId)
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid invitationId)
    {
        var invitation = await _context.FriendInvitations
            .FirstOrDefaultAsync(i => i.InvitationId == invitationId);

        if (invitation != null)
        {
            _context.FriendInvitations.Remove(invitation);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid senderId, Guid receiverId)
    {
        return await _context.FriendInvitations
            .AnyAsync(i => i.SenderId == senderId && i.ReceiverId == receiverId);
    }
}
