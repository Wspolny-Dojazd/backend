using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Represents crud operations for group members.
/// </summary>
/// <param name="databaseContext">The database context used to access group data.</param>
public class GroupMemberRepository(DatabaseContext databaseContext)
    : IGroupMemberRepository
{
    /// <inheritdoc/>
    public async Task<GroupMember?> GetGroupMemberbyIdAsync(Guid userId, int groupId)
    {
        return await databaseContext.GroupMembers
            .FirstOrDefaultAsync(gm => gm.UserId == userId && gm.GroupId == groupId);
    }

    /// <inheritdoc/>
    public async Task<List<GroupMember>> GetGroupMembersAsync(int groupId)
    {
        return await databaseContext.GroupMembers
            .Include(gm => gm.UserId)
            .Where(gm => gm.GroupId == groupId)
            .ToListAsync();
    }
}
