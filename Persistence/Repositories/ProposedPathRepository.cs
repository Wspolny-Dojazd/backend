using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Provides data access operations for <see cref="ProposedPath"/> entities.
/// </summary>
/// <param name="databaseContext">The database context.</param>
public class ProposedPathRepository(DatabaseContext databaseContext)
    : IProposedPathRepository
{
    /// <inheritdoc/>
    public async Task AddRangeAsync(IEnumerable<ProposedPath> paths)
    {
        await databaseContext.ProposedPaths.AddRangeAsync(paths);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<ProposedPath?> GetByIdAsync(Guid id)
    {
        return await databaseContext.ProposedPaths
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc/>
    public async Task<List<ProposedPath>> GetAllByGroupIdAsync(int groupId)
    {
        return await databaseContext.ProposedPaths
            .Where(p => p.GroupId == groupId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(ProposedPath proposedPath)
    {
        _ = databaseContext.ProposedPaths.Update(proposedPath);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task RemoveRangeAsync(IEnumerable<ProposedPath> proposedPaths)
    {
        databaseContext.ProposedPaths.RemoveRange(proposedPaths);
        _ = await databaseContext.SaveChangesAsync();
    }
}
