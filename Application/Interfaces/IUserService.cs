using Application.DTOs;
using Application.Exceptions;
using Domain.Model;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user data transfer object by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user data transfer object.</returns>
    Task<UserDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user entity by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user entity.</returns>
    /// <exception cref="UserNotFoundException">
    /// Thrown when the user is not in the database.
    /// </exception>
    /// <remarks>
    /// This method also checks if the user is in the database.
    /// </remarks>
    Task<User> GetEntityByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a list of users by their unique nicknames or usernames.
    /// </summary>
    /// <param name="query">The string that is used to measure Levenstein distance between different usersnames or nicknames.</param>
    /// <returns>The list of users data.</returns>
    /// <exception cref="UserNotFoundException">
    /// Thrown when the user is not in the database.
    /// </exception>
    Task<List<UserDto>> SearchByUsernameOrNicknameAsync(string query);

    /// <summary>
    /// Retrieves a list of all users from the database.
    /// </summary>
    /// <returns>The list of all of users data.</returns>
    Task<List<UserDto>> GetAllAsync();
}
