using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

/// <summary>
/// Represents user operations with user repository.
/// </summary>
public class GroupService : IGroupService
{
    private readonly IGroupRepository groupRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupService"/> class.
    /// </summary>
    /// <param name="groupRepository">User repository that allows database operations on user table.</param>
    public GroupService(IGroupRepository groupRepository, IMapper mapper)
    {
        this.groupRepository = groupRepository;
        this.mapper = mapper;
    }

    /// <summary>
    /// Method that gets user display data.
    /// </summary>
    /// <param name="id">Unique user identifier.</param>
    /// <returns>Returns user display data.</returns>
    public async Task<GroupDto> GetGroupByIdAsync(int id)
    {
        var group = await this.groupRepository.GetGroupByIdAsync(id);

        return this.mapper.Map<Group, GroupDto>(group);
    }
}
