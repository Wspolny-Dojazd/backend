using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="UserLocation"/> data access operations.
/// </summary>
public interface IUserLocationRepository
{
    /// <summary>
    /// Retrieves a current location of the user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The user location if found; otherwise <see langword="null"/>.</returns>
    Task<UserLocation?> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Sets a user's location.
    /// </summary>
    /// <param name="userLocation">The user location to set.</param>"
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(UserLocation userLocation);

    /// <summary>
    /// Updates or creates a user's location.
    /// </summary>
    /// <param name="userLocation">The user location to update or create.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(UserLocation userLocation);
}
