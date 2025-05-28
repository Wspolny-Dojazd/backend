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
                .ThenInclude(gm => gm.UserLocation)
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
    public async Task RemoveAsync(Group group)
    {
        _ = databaseContext.Groups.Remove(group);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task AddUserAsync(Group group, User user)
    {
        group.GroupMembers.Add(user);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task RemoveUserAsync(Group group, User user)
    {
        _ = group.GroupMembers.Remove(user);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<string> GenerateUniqueJoiningCodeAsync()
    {
        const string digits = "0123456789";
        string joiningCode;
        bool isUnique;

        do
        {
            joiningCode = new string([.. Enumerable.Range(0, 6).Select(_ => digits[Random.Next(digits.Length)])]);
            isUnique = !await databaseContext.Groups.AnyAsync(g => g.JoiningCode == joiningCode);
        }
        while (!isUnique);

        return joiningCode;
    }

    /// <inheritdoc/>
    public async Task<List<Group>> GetGroupsByUserIdAsync(Guid userId)
    {
        return await databaseContext.Groups
            .Where(g => g.GroupMembers.Any(m => m.Id == userId))
            .Include(g => g.GroupMembers)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> HasMemberAsync(int groupId, Guid userId)
    {
        return await databaseContext.Groups
            .Where(g => g.Id == groupId)
            .SelectMany(g => g.GroupMembers)
            .AnyAsync(m => m.Id == userId);
    }

    /// <inheritdoc/>
    public async Task<bool> IsOwnerAsync(int groupId, Guid userId)
    {
        return await databaseContext.Groups
            .Where(g => g.Id == groupId)
            .Select(g => g.Creator)
            .AnyAsync(m => m.Id == userId);
    }
}
