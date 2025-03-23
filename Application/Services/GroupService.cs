using Application.DTOs;
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
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupService"/> class.
    /// </summary>
    /// <param name="groupRepository">Group repository that allows database operations on group table.</param>
    public GroupService(IGroupRepository groupRepository, IMapper mapper)
    {
        this.groupRepository = groupRepository;
        this.mapper = mapper;
    }

    /// <summary>
    /// Method that gets group display data.
    /// </summary>
    /// <param name="id">Unique group identifier.</param>
    /// <returns>Returns group display data.</returns>
    public async Task<GroupDto> GetGroupByIdAsync(int id)
    {
        var group = await this.groupRepository.GetGroupByIdAsync(id);

        return this.mapper.Map<Group, GroupDto>(group);
    }

    public async Task<GroupDto> CreateGroupAsync()
    {
        var group = await this.groupRepository.CreateGroupAsync();

        return this.mapper.Map<Group, GroupDto>(group);
    }

}
