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
    public async Task<GroupDto> GetByIdAsync(int id)
    {
        var group = await this.groupRepository.GetByIdAsync(id)
                    ?? throw new GroupNotFoundException(id);

        return this.mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> CreateAsync()
    {
        var group = new Group
        {
            JoiningCode = await this.groupRepository.GenerateUniqueJoiningCodeAsync(),
            Routes = new List<Route>(),
            LiveLocations = new List<Location>(),
            GroupMembers = new List<User>(),
        };
        await this.groupRepository.AddAsync(group);

        return this.mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> AddUserByCodeAsync(string code, int userId)
    {
        var group = await this.groupRepository.GetByCodeAsync(code)
                    ?? throw new GroupNotFoundException(code);
        var user = await this.userRepository.GetByIdAsync(userId)
                    ?? throw new UserNotFoundException(userId);

        if (group.GroupMembers.Contains(user))
        {
            throw new UserAlreadyInGroupException(group.Id, userId);
        }

        await this.groupRepository.AddUserAsync(group, user);
        return this.mapper.Map<Group, GroupDto>(group);
    }

    /// <inheritdoc/>
    public async Task<GroupDto> RemoveUserAsync(int id, int userId)
    {
        var group = await this.groupRepository.GetByIdAsync(id)
                    ?? throw new GroupNotFoundException(id);
        var user = await this.userRepository.GetByIdAsync(userId)
                    ?? throw new UserNotFoundException(userId);
        if (!group.GroupMembers.Contains(user))
        {
            throw new UserNotInGroupException(id, userId);
        }

        await this.groupRepository.RemoveUserAsync(group, user);
        return this.mapper.Map<Group, GroupDto>(group);
    }
}
