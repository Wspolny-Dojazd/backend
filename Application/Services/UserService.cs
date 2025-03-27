using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

/// <summary>
/// Provides user-related business logic.
/// </summary>
/// <param name="userRepository">The repository for accessing user data.</param>
/// <param name="mapper">The object mapper.</param>
public class UserService(IUserRepository userRepository, IMapper mapper)
    : IUserService
{
    /// <inheritdoc/>
    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var user = await userRepository.GetUserByIdAsync(id)
            ?? throw new UserNotFoundException(id);

        return mapper.Map<User, UserDto>(user);
    }
}
