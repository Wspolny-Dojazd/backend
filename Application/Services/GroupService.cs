using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

/// <summary>
/// Represents group operations with group repository.
/// </summary>
public class GroupService : IGroupService
{
    private readonly IGroupRepository groupRepository;
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupService"/> class.
    /// </summary>
    /// <param name="groupRepository">Group repository that allows database operations on group table.</param>
    /// /// <param name="userRepository">User repository that allows database operations on user table.</param>
    /// <param name="mapper">The object mapper.</param>
    public GroupService(IGroupRepository groupRepository, IUserRepository userRepository, IMapper mapper)
    {
        this.groupRepository = groupRepository;
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    /// <summary>
    /// Method that gets group display data.
    /// </summary>
    /// <param name="id">Unique group identifier.</param>
    /// <returns>Returns group display data.</returns>
    public async Task<GroupDto> GetGroupByIdAsync(int id)
    {
        var group = await this.groupRepository.GetGroupByIdAsync(id)
                    ?? throw new GroupByIdNotFoundException(id);

        return this.mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> CreateGroupAsync()
    {
        var group = await this.groupRepository.CreateGroupAsync();

        return this.mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> AddUserViaCodeAsync(string code, int userId)
    {
        var group = await this.groupRepository.GetGroupByCodeAsync(code)
                    ?? throw new GroupByCodeNotFoundException(code);
        var user = await this.userRepository.GetByIdAsync(userId)
                    ?? throw new UserNotFoundException(userId);

        if (!group.GroupMembers.Contains(user))
        {
            group.GroupMembers.Add(user);
            user.Groups.Add(group);
            this.groupRepository.SaveAsync();
        }
        else
        {
            throw new UserAlreadyInGroupExeption(group.Id, userId);
        }

        return this.mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> RemoveUserFromGroupAsync(int id, int userId)
    {
        var group = await this.groupRepository.GetGroupByIdAsync(id)
                    ?? throw new GroupByIdNotFoundException(id);
        var user = await this.userRepository.GetByIdAsync(userId)
                    ?? throw new UserNotFoundException(userId);

        if (group.GroupMembers.Contains(user))
        {
            _ = group.GroupMembers.Remove(user);
            _ = user.Groups.Remove(group);
            this.groupRepository.SaveAsync();
        }
        else
        {
            throw new UserNotInGroupExeption(id, userId);
        }

        return this.mapper.Map<Group, GroupDto>(group);
    }
}
