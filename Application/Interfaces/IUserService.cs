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
}
