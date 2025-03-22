using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Services;

/// <summary>
/// Represents user operations with user repository.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">User repository that allows database operations on user table.</param>
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    /// <summary>
    /// Method that gets user display data.
    /// </summary>
    /// <param name="id">Unique user identifier.</param>
    /// <returns>Returns user display data.</returns>
    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var user = await this.userRepository.GetUserByIdAsync(id);

        return this.mapper.Map<User, UserDto>(user);
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
