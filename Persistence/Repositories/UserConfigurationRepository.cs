using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Provides data access operations for <see cref="UserConfiguration"/> entities.
/// </summary>
/// <param name="databaseContext">The database context used to access user configuration data.</param>
public class UserConfigurationRepository(DatabaseContext databaseContext)
    : IUserConfigurationRepository
{
    /// <inheritdoc/>
    public async Task<UserConfiguration?> GetByUserIdAsync(int userId)
    {
        return await databaseContext.UserConfigurations
            .FirstOrDefaultAsync(conf => conf.UserId == userId);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(UserConfiguration configuration)
    {
        _ = databaseContext.UserConfigurations.Update(configuration);
        _ = await databaseContext.SaveChangesAsync();
    }
}
