using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Services;

/// <summary>
/// Provides user-related logic.
/// </summary>
/// <param name="userRepository">The repository for accessing user data.</param>
/// <param name="mapper">The object mapper.</param>
public class UserService(IUserRepository userRepository, IMapper mapper)
    : IUserService
{
    /// <inheritdoc/>
    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await this.GetEntityByIdAsync(id);
        return mapper.Map<User, UserDto>(user);
    }

    /// <inheritdoc/>
    public async Task<User> GetEntityByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id)
            ?? throw new UserNotFoundException(id);

        return user;
    }

    /// <summary>
    /// Method that gets users display data.
    /// </summary>
    /// <param name="value">String by which the users are found.</param>
    /// <returns>Returns users display data.</returns>
    public async Task<List<UserDto>> GetUsersByNicknameAsync(string value)
    {
        var users = await this.userRepository.GetAllUsersAsync();
        var filteredUsers = users.Where(t => t.Nickname.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
        return this.mapper.Map<List<User>, List<UserDto>>(filteredUsers);
    }

    /// <summary>
    /// Method that deletes the user.
    /// </summary>
    /// <param name="nickname">Unique user nickname.</param>
    public async Task DeleteUserByNicknameAsync(string nickname)
    {
       var user = await this.userRepository.GetUserByNicknameAsync(nickname);
       await this.userRepository.DeleteUserAsync(user);
    }
}
