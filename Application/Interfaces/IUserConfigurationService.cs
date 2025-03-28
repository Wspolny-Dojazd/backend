using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for user-related operations.
/// </summary>
public interface IUserConfigurationService
{
    /// <summary>
    /// Retrieves a user by its unique identifier.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>
    /// The user if found; otherwise, <c>null</c>.
    /// </returns>
    Task<UserConfigurationDto> GetByUserIdAsync(int userId);

    /// <summary>
    /// Retrieves a user by its unique identifier.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <param name="dto">The ID of thsdrfln;kwsdrhjklghikujdsfghjkfgjkhfdse user to retrieve.</param>
    /// <returns>
    /// The user if found; otherwise, <c>null</c>.
    /// </returns>
    Task UpdateAsync(int userId, UserConfigurationDto dto);
}
