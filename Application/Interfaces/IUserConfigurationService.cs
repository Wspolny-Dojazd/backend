using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract related to user configuration.
/// </summary>
public interface IUserConfigurationService
{
    /// <summary>
    /// Retrieves a user configuration by the unique identifier of the user.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve configuration for.</param>
    /// <returns>The user configuration.</returns>
    Task<UserConfigurationDto> GetByUserIdAsync(int userId);

    /// <summary>
    /// Updates user configuration.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve configuration for.</param>
    /// <param name="dto">The configuration data to update.</param>
    /// <returns>The updated user configuration.</returns>
    Task<UserConfigurationDto> UpdateAsync(int userId, UserConfigurationDto dto);
}
