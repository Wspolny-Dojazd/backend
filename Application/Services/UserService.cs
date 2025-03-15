using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

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
}
