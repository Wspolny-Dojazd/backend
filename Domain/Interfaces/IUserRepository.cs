using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="User"/> data access operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user if found; otherwise, <see langword="null"/>.</returns>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user by its email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The user if found; otherwise, <see langword="null"/>.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Retrieves a user by its username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user if found; otherwise, <see langword="null"/>.</returns>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// Adds a new user to the database.
    /// </summary>
    /// <param name="user">The user to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(User user);

    /// <summary>
    /// Updates an existing user in the database.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(User user);

    /// <summary>
    /// Retrieves a list of all users from the database.
    /// </summary>
    /// <returns>A list of all users.</returns>
    Task<List<User>> GetAllAsync();
}
