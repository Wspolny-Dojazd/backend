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
    /// Creates a friendship relationship between two users.
    /// </summary>
    /// <param name="userId1">The ID of the first user in the friendship.</param>
    /// <param name="userId2">The ID of the second user in the friendship.</param>
    /// <returns>A task representing the asynchronous operation of adding the users as friends.</returns>
    /// <remarks>
    /// This method establishes a bidirectional friendship relationship,
    /// meaning both users will appear in each other's friends list.
    /// If the users are already friends, this operation should be idempotent.
    /// </remarks>
    Task AddFriendAsync(Guid userId1, Guid userId2);
}
