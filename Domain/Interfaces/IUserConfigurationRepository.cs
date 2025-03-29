using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for user configuration data access operations.
/// </summary>
public interface IUserConfigurationRepository
{
    /// <summary>
    /// Retrieves a user configuration by its unique identifier.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve configuration for.</param>
    /// <returns>The user configuration if found; otherwise, <see langword="null"/>.</returns>
    Task<UserConfiguration?> GetByUserIdAsync(int userId);

    /// <summary>
    /// Updates user configuration in database.
    /// </summary>
    /// <param name="configuration">The updated user configuration data to be saved in the database.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(UserConfiguration configuration);
}
