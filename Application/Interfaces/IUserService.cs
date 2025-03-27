using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user data.</returns>
    Task<UserDto> GetUserByIdAsync(int id);
}
