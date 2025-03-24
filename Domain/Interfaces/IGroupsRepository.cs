using Domain.Model;

namespace Domain.Interfaces;

public interface IGroupRepository
{
    Task<Group> GetGroupByIdAsync(int id);
    Task<Group> CreateGroupAsync();
    Task<Group> AddUserViaCodeAsync(string code, int userId);
    Task<Group> RemoveUserFromGroupAsync(int id, int userId);
}
