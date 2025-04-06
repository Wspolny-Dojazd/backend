using Application.DTOs.UserLocation;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract related to user location.
/// </summary>
public interface IUserLocationService
{
    /// <summary>
    /// Updates or creates a user's location.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="dto">The location data of the user.</param>
    /// <returns>The updated user location.</returns>
    Task<UserLocationRequestDto> UpdateAsync(Guid userId, UserLocationRequestDto dto);

    /// <summary>
    /// Retrieves the location of the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The location of the user.</returns>
    Task<UserLocationRequestDto> GetByUserIdAsync(Guid userId);
}
