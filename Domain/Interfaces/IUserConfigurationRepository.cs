using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines data access operations for <see cref="User"/> entities.
/// </summary>
public interface IUserConfigurationRepository
{
    /// <summary>
    /// Retrieves a user entity by its unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The user entity if found; otherwise, <c>null</c>.</returns>
    Task<UserConfiguration?> GetByUserIdAsync(int userId);

    /// <summary>
    /// Retrieves a user entity by its unique identifier.
    /// </summary>
    /// <param name="configuration">The unasdfasdfique identifier of the user.</param>
    /// <returns>The user entity if found; otherwise, <c>null</c>.</returns>
    Task UpdateAsync(UserConfiguration configuration);
}
