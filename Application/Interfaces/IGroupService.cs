using Application.DTOs;

namespace Application.Interfaces;

public interface IGroupService
{
    Task<GroupDto> GetGroupByIdAsync(int id);
    Task<GroupDto> CreateGroupAsync();
    Task<GroupDto> AddUserViaCodeAsync(string code, int userId);
    Task<GroupDto> RemoveUserFromGroupAsync(int id, int userId);
}
