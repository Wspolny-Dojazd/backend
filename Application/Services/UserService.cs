using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

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
    public async Task<IEnumerable<UserDto>> SearchByUsernameOrNicknameAsync(string query)
    {
        const int threshold = 2;
        var users = await userRepository.GetAllAsync();
        query = query.ToLowerInvariant();

        IEnumerable<User> result = users
        .Select(user => new
        {
            User = user,
            DistanceUsername = Fastenshtein.Levenshtein.Distance(query, user.Username.ToLowerInvariant()),
            DistanceNickname = Fastenshtein.Levenshtein.Distance(query, user.Nickname.ToLowerInvariant()),
        })
        .Where(x => x.DistanceUsername <= threshold || x.DistanceNickname <= threshold)
        .OrderBy(x => x.DistanceUsername)
        .ThenBy(x => x.DistanceNickname)
        .Select(x => x.User);

        return mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(result);
    }
}
