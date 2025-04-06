using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Provides data access operations for <see cref="UserLocation"/> entities.
/// </summary>
/// <param name="databaseContext">The database context used to access user location data.</param>
public class UserLocationRepository(DatabaseContext databaseContext)
    : IUserLocationRepository
{
    /// <inheritdoc/>
    public async Task<UserLocation?> GetByUserIdAsync(Guid userId)
    {
        return await databaseContext.UserLocations
            .FirstOrDefaultAsync(l => l.UserId == userId);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(UserLocation userLocation)
    {
        _ = databaseContext.UserLocations.Update(userLocation);
        _ = await databaseContext.SaveChangesAsync();
    }
}
