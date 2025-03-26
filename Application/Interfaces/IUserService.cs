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
    /// <returns>
    /// The user if found; otherwise, <c>null</c>.
    /// </returns>
    Task<UserDto> GetUserByIdAsync(int id);
}
