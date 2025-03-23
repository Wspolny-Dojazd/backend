using Domain.Model;

namespace Domain.Interfaces;

public interface IGroupRepository
{
    Task<Group> GetGroupByIdAsync(int id);
}
