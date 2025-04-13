using Microsoft.EntityFrameworkCore;
using PublicTransportService.Domain.Entities;
using PublicTransportService.Domain.Interfaces;

namespace PublicTransportService.Infrastructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="Trip"/> entities.
/// </summary>
/// <param name="dbContext">The database context for accessing the database.</param>
internal class TripRepository(PTSDbContext dbContext)
    : ITripRepository
{
    /// <inheritdoc/>
    public async Task<Trip?> GetByIdAsync(string id)
    {
        return await dbContext.Trips
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new KeyNotFoundException($"Trip with ID {id} not found.");
    }
}
