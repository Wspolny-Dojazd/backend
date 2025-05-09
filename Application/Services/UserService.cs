using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.Extensions.DependencyInjection;

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

    /// <inheritdoc/>
    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await userRepository.GetAllAsync();
        return mapper.Map<List<User>, List<UserDto>>(users);
    }

    /// <inheritdoc/>
    public async Task<List<UserDto>> SearchByUsernameOrNicknameAsync(string query)
    {
        var users = await userRepository.GetAllAsync();

        IEnumerable<User> result =
        users.Where(user =>
        {
            int levenshteinDistance_u = Fastenshtein.Levenshtein.Distance(query.ToLower(), user.Username.ToLower());
            int levenshteinDistance_n = Fastenshtein.Levenshtein.Distance(query.ToLower(), user.Nickname.ToLower());
            return levenshteinDistance_u <= 2 || levenshteinDistance_n <= 2;
        });
        return mapper.Map<IEnumerable<User>, List<UserDto>>(result);
    }
}
