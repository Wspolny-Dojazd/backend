using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Represents crud operations for user.
/// </summary>
/// <param name="databaseContext">The database context used to access user data.</param>
public class UserConfigurationRepository(DatabaseContext databaseContext)
    : IUserConfigurationRepository
{
    /// <inheritdoc/>
    public async Task<UserConfiguration?> GetByUserIdAsync(int userId)
    {
        return await databaseContext.UserConfigurations
            .Where(conf => conf.UserId == userId)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(UserConfiguration configuration)
    {
        _ = databaseContext.UserConfigurations.Update(configuration);
        _ = await databaseContext.SaveChangesAsync();
    }
}
