using Application.DTOs;
using Domain.Model;

namespace Application.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Method that gets user display data.
    /// </summary>
    /// <param name="id">Unique user identifier.</param>
    /// <returns>Returns user display data.</returns>
    Task<UserDto> GetUserByIdAsync(int id);

    /// <summary>
    /// Method that deletes the user.
    /// </summary>
    /// <param name="nickname">Unique user nickname.</param>
    Task DeleteUserByNicknameAsync(string nickname);

    /// <summary>
    /// Method that gets users display data.
    /// </summary>
    /// <param name="value">String by which the users are found.</param>
    /// <returns>Returns users display data.</returns>
    Task<List<UserDto>> GetUsersByNicknameAsync(string value);
}
