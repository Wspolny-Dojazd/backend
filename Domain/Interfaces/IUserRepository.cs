using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines data access operations for <see cref="User"/> entities.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user entity if found; otherwise, <c>null</c>.</returns>
    Task<User?> GetUserByIdAsync(int id);
}
