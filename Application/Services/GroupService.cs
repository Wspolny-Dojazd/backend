using Application.DTOs;
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
    IGroupMemberRepository groupmemberRepository,
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
        var creator = await userRepository.GetByIdAsync(creatorId)
            ?? throw new UserNotFoundException(creatorId);

        var group = new Group
        {
            JoiningCode = await groupRepository.GenerateUniqueJoiningCodeAsync(),
            Routes = [],
            LiveLocations = [],
            GroupMembers = [],
            CreatorId = creatorId,
            Creator = creator,
        };
        await groupRepository.AddAsync(group);
        return mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> AddUserByCodeAsync(string code, Guid userId)
    {
        var group = await groupRepository.GetByCodeAsync(code)
            ?? throw new GroupNotFoundException(code);

        var groupmember = await groupmemberRepository.GetGroupMemberbyIdAsync(userId, group.Id)
            ?? throw new UserNotFoundException(userId);

        if (group.GroupMembers.Any(gm => gm.UserId == userId))
        {
            throw new UserAlreadyInGroupException(group.Id, userId);
        }

        await groupRepository.AddGroupMemberAsync(group, groupmember);
        return mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> RemoveUserAsync(int id, Guid userId)
    {
        var group = await groupRepository.GetByIdAsync(id)
            ?? throw new GroupNotFoundException(id);

        var groupmember = await groupmemberRepository.GetGroupMemberbyIdAsync(userId, id)
            ?? throw new UserNotFoundException(userId);

        if (!group.GroupMembers.Any(gm => gm.UserId == userId))
        {
            throw new UserNotInGroupException(id, userId);
        }

        await groupRepository.RemoveGroupMemberAsync(group, groupmember);
        return mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<GroupDto>> GetGroupsForUserAsync(Guid userId)
    {
        var groups = await groupRepository.GetGroupsByUserIdAsync(userId);
        return mapper.Map<IEnumerable<Group>, IEnumerable<GroupDto>>(groups);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<GroupMemberDto>> GetGroupMembersAsync(int groupId)
    {
        var group = await groupRepository.GetByIdAsync(groupId)
            ?? throw new GroupNotFoundException(groupId);

        var members = group.GroupMembers;
        return mapper.Map<IEnumerable<GroupMember>, IEnumerable<GroupMemberDto>>(members);
    }
}
