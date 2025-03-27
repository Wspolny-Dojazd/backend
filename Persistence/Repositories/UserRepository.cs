using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Represents crud operations for user.
/// </summary>
/// <param name="databaseContext">The database context used to access user data.</param>
public class UserRepository(DatabaseContext databaseContext)
    : IUserRepository
{
    /// <inheritdoc/>
    public async Task<User> GetUserByIdAsync(int id)
    {
        return await databaseContext.Users
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await this.databaseContext.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task InsertUserAsync(User user)
    {
        _ = await this.databaseContext.Users.AddAsync(user);
        _ = await this.databaseContext.SaveChangesAsync();

    }

    public async Task UpdateUserAsync(User user)
    {
        _ = this.databaseContext.Users.Update(user);
        _ = await this.databaseContext.SaveChangesAsync();

    }

}
