using Domain.Model;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);
}
