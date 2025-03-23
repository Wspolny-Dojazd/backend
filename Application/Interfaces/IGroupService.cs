using Application.DTOs;

namespace Application.Interfaces;

public interface IGroupService
{
    Task<GroupDto> GetGroupByIdAsync(int id);
    Task<GroupDto> CreateGroupAsync();
}
