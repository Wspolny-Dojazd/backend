using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="UserLocation"/> data access operations.
/// </summary>
public interface IUserLocationRepository
{
    /// <summary>
    /// Retrieves the current location of the user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The user location if found; otherwise, <see langword="null"/>.</returns>
    Task<UserLocation?> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Inserts a new user location or updates the existing one.
    /// </summary>
    /// <param name="userLocation">The user location to insert or update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(UserLocation userLocation);
}
