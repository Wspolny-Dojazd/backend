﻿using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

/// <summary>
/// Provides group-related operations.
/// </summary>
/// <param name="groupRepository">The repository for accessing group data.</param>
/// <param name="userRepository">The repository for accessing user data.</param>
/// <param name="mapper">The object mapper.</param>
public class GroupService(
    IGroupRepository groupRepository,
    IUserRepository userRepository,
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
    public async Task<GroupDto> CreateAsync()
    {
        var group = new Group
        {
            JoiningCode = await groupRepository.GenerateUniqueJoiningCodeAsync(),
            Routes = [],
            LiveLocations = [],
            GroupMembers = [],
        };

        await groupRepository.AddAsync(group);
        return mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> AddUserByCodeAsync(string code, int userId)
    {
        var group = await groupRepository.GetByCodeAsync(code)
            ?? throw new GroupNotFoundException(code);

        var user = await userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException(userId);

        if (group.GroupMembers.Contains(user))
        {
            throw new UserAlreadyInGroupException(group.Id, userId);
        }

        await groupRepository.AddUserAsync(group, user);
        return mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> RemoveUserAsync(int id, int userId)
    {
        var group = await groupRepository.GetByIdAsync(id)
            ?? throw new GroupNotFoundException(id);

        var user = await userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException(userId);

        if (!group.GroupMembers.Contains(user))
        {
            throw new UserNotInGroupException(id, userId);
        }

        await groupRepository.RemoveUserAsync(group, user);
        return mapper.Map<Group, GroupDto>(group);
    }
}
