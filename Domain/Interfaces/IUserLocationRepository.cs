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
    /// Adds a new user location entry.
    /// </summary>
    /// <param name="userLocation">The user location to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(UserLocation userLocation);

    /// <summary>
    /// Updates an existing user location entry.
    /// </summary>
    /// <param name="userLocation">The user location to update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(UserLocation userLocation);
}
