using Microsoft.EntityFrameworkCore;
using PublicTransportService.Domain.Entities;
using PublicTransportService.Domain.Interfaces;

namespace PublicTransportService.Infrastructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="Stop"/> entities.
/// </summary>
/// <param name="dbContext">The database context for accessing the database.</param>
internal class StopRepository(PTSDbContext dbContext) : IStopRepository
{
    /// <inheritdoc/>
    public async Task<Stop?> GetByIdAsync(string id)
    {
        return await dbContext.Stops.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public async Task<List<Stop>> GetByIdsAsync(IEnumerable<string> ids)
    {
        return await dbContext.Stops
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<string> GetNearestLogicalIdStopAsync(double latitude, double longitude)
    {
        return await dbContext.Stops
            .OrderBy(stop =>
                Math.Pow(stop.Latitude - latitude, 2) +
                Math.Pow(stop.Longitude - longitude, 2))
            .Select(stop => stop.LogicalId)
            .FirstAsync();
    }
}
