using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Provides data access operations for <see cref="GroupPath"/> entities.
/// </summary>
/// <param name="databaseContext">The application's database context.</param>
public class GroupPathRepository(DatabaseContext databaseContext)
    : IGroupPathRepository
{
    /// <inheritdoc/>
    public async Task AddAsync(GroupPath groupPath)
    {
        _ = await databaseContext.GroupPaths.AddAsync(groupPath);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<GroupPath?> GetByGroupIdAsync(int groupId)
    {
        return await databaseContext.GroupPaths
            .FirstOrDefaultAsync(p => p.GroupId == groupId);
    }
}
