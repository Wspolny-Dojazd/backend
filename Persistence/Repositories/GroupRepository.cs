using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Represents crud operations for group.
/// </summary>
/// <param name="databaseContext">The database context used to access group data.</param>
public class GroupRepository(DatabaseContext databaseContext)
    : IGroupRepository
{
    private static readonly Random Random = new();

    /// <inheritdoc/>
    public async Task<Group?> GetByIdAsync(int id)
    {
        return await databaseContext.Groups
            .Include(g => g.GroupMembers)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <inheritdoc/>
    public async Task<Group?> GetByCodeAsync(string code)
    {
        return await databaseContext.Groups
            .Include(g => g.GroupMembers)
            .FirstOrDefaultAsync(g => g.JoiningCode == code);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Group group)
    {
        _ = await databaseContext.Groups.AddAsync(group);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task AddGroupMemberAsync(Group group, GroupMember groupmember)
    {
        group.GroupMembers.Add(groupmember);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task RemoveGroupMemberAsync(Group group, GroupMember groupmember)
    {
        _ = group.GroupMembers.Remove(groupmember);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<string> GenerateUniqueJoiningCodeAsync()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string joiningCode;

        bool isUnique;

        do
        {
            joiningCode = new string(Enumerable.Range(0, 6)
                .Select(_ => chars[Random.Next(chars.Length)])
                .ToArray());

            isUnique = !await databaseContext.Groups.AnyAsync(g => g.JoiningCode == joiningCode);
        } while (!isUnique);

        return joiningCode;
    }

    /// <inheritdoc/>
    public async Task<List<Group>> GetGroupsByUserIdAsync(Guid userId)
    {
        return await databaseContext.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Groups)
            .Distinct()
            .ToListAsync();
    }
}
