using Domain.Model;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);

    Task InsertUserAsync(User user);

    Task UpdateUserAsync(User user);

    Task<User> GetUserByEmailAsync(string email);
}
