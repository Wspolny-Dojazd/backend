using Application.DTOs;
using Application.DTOs.UserLocation;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Shared.Enums.ErrorCodes;

namespace Application.Services;

/// <summary>
/// Provides group-related operations.
/// </summary>
/// <param name="groupRepository">The repository for accessing group data.</param>
/// <param name="userService">The service that handles user-related logic.</param>
/// <param name="mapper">The object mapper.</param>
public class GroupService(
    IGroupRepository groupRepository,
    IUserService userService,
    IMapper mapper)
    : IGroupService
{
    /// <inheritdoc/>
    public async Task<GroupDto> GetByIdAsync(int id)
    {
        var group = await groupRepository.GetByIdAsync(id)
            ?? throw new GroupNotFoundException(id);

        return mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> CreateAsync(Guid creatorId)
    {
        var creator = await userService.GetEntityByIdAsync(creatorId);

        var group = new Group
        {
            JoiningCode = await groupRepository.GenerateUniqueJoiningCodeAsync(),
            CreatorId = creatorId,
            GroupMembers = [creator],
        };

        await groupRepository.AddAsync(group);
        return mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> AddUserByCodeAsync(string code, Guid userId)
    {
        var group = await groupRepository.GetByCodeAsync(code)
            ?? throw new GroupNotFoundException(code);

        var user = await userService.GetEntityByIdAsync(userId);

        if (group.GroupMembers.Contains(user))
        {
            throw new AppException(400, GroupErrorCode.USER_ALREADY_IN_GROUP);
        }

        await groupRepository.AddUserAsync(group, user);
        return mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto?> RemoveUserAsync(int groupId, Guid userId)
    {
        var group = await groupRepository.GetByIdAsync(groupId)
            ?? throw new GroupNotFoundException(groupId);

        var user = await userService.GetEntityByIdAsync(userId);

        if (group.CreatorId == userId)
        {
            await groupRepository.RemoveAsync(group);
            return null;
        }

        await groupRepository.RemoveUserAsync(group, user);
        return mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> KickUserAsync(int groupId, Guid userId)
    {
        var group = await groupRepository.GetByIdAsync(groupId)
            ?? throw new GroupNotFoundException(groupId);

        var user = await userService.GetEntityByIdAsync(userId);

        if (group.CreatorId == userId)
        {
            throw new AppException(403, GroupErrorCode.ACCESS_DENIED);
        }

        await groupRepository.RemoveUserAsync(group, user);
        return mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<GroupDto>> GetGroupsForUserAsync(Guid userId)
    {
        _ = await userService.GetEntityByIdAsync(userId);

        var groups = await groupRepository.GetGroupsByUserIdAsync(userId);
        return mapper.Map<IEnumerable<Group>, IEnumerable<GroupDto>>(groups);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<GroupMemberDto>> GetGroupMembersAsync(int groupId)
    {
        var group = await groupRepository.GetByIdAsync(groupId)
            ?? throw new GroupNotFoundException(groupId);

        var members = group.GroupMembers;
        return members.Select(user => new GroupMemberDto(
            user.Id, user.Username, user.Nickname, mapper.Map<UserLocation?, UserLocationDto?>(user.UserLocation))
            { IsCreator = group.CreatorId == user.Id });
    }
}
