using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Shared.Enums.ErrorCodes;

namespace Application.Services;

/// <summary>
/// Provides operations for managing friend relationships.
/// </summary>
/// <param name="userService">The service for managing user-related operations.</param>
/// <param name="userRepository">The repository for accessing user data.</param>
/// <param name="mapper">The object mapper.</param>
public class FriendService(
    IUserService userService,
    IUserRepository userRepository,
    IFriendRepository friendRepository,
    IMapper mapper)
    : IFriendService
{
    /// <inheritdoc/>
    public async Task CreateFriendshipAsync(Guid userId, Guid friendId)
    {
        var user = await userService.GetEntityByIdAsync(userId);
        var friend = await userService.GetEntityByIdAsync(friendId);

        if (user.Friends.Any(f => f.Id == friendId))
        {
            throw new AppException(400, FriendInvitationErrorCode.ALREADY_FRIEND);
        }

        user.Friends.Add(friend);
        friend.Friends.Add(user);

        await userRepository.UpdateAsync(user);
        await userRepository.UpdateAsync(friend);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserDto>> GetFriendsAsync(Guid userId)
    {
        var user = await userService.GetEntityByIdAsync(userId);
        return mapper.Map<IEnumerable<UserDto>>(user.Friends);
    }

    /// <inheritdoc/>
    public async Task<bool> AreFriendsAsync(Guid userId, Guid friendId)
    {
        var user = await userService.GetEntityByIdAsync(userId);
        return user.Friends.Any(f => f.Id == friendId);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserDto>> GetAllAsync(Guid userId)
    {
        var users = await friendRepository.GetAllAsync(userId);
        return mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
    }
}
