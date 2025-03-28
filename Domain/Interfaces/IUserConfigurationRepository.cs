using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines data access operations for <see cref="UserConfiguration"/> entities.
/// </summary>
public interface IUserConfigurationRepository
{
    /// <summary>
    /// Retrieves a user configuration by its unique identifier.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve configuration for.</param>
    /// <returns>
    /// The user configuration if found; otherwise, <c>null</c>.
    /// </returns>
    Task<UserConfiguration?> GetByUserIdAsync(int userId);

    /// <summary>
    /// Updates user configuration in database.
    /// </summary>
    /// <param name="configuration">The updated user configuration data to be saved in the database.</param>
    /// <returns>
    /// A task representing the asynchronous update operation. This method does not return any value.
    /// </returns>
    Task UpdateAsync(UserConfiguration configuration);
}
