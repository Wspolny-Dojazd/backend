using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract related to user configuration.
/// </summary>
public interface IUserConfigurationService
{
    /// <summary>
    /// Retrieves a user configuration by its unique identifier.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve configuration for.</param>
    /// <returns>
    /// The user configuration if found; otherwise, <c>null</c>.
    /// </returns>
    Task<UserConfigurationDto> GetByUserIdAsync(int userId);

    /// <summary>
    /// Updates user configuration.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve configuration for.</param>
    /// <param name="dto">The configuration data to update.</param>
    /// <returns>
    /// A task representing the asynchronous update operation. This method does not return any value.
    /// </returns>
    Task UpdateAsync(int userId, UserConfigurationDto dto);
}
