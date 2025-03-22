using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Represents crud operations for user.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="databaseContext">Database context that allows to get and manipulate database data.</param>
    public UserRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    /// <summary>
    /// This async method get user's data by id from database.
    /// </summary>
    /// <param name="id">User's id.</param>
    /// <returns>User's data from database.</returns>
    public async Task<User> GetUserByIdAsync(int id)
    {
        return await this.databaseContext.Users
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// This async method get all users' data by from database.
    /// </summary>
    /// <returns>All users' data from database.</returns>
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await this.databaseContext.Users.ToListAsync();
    }

    /// <summary>
    /// This async method deletes user's data from database.
    /// </summary>
    /// <param name="user">User's data.</param>
    public async Task DeleteUserAsync(User user)
    {
        this.databaseContext.Users.Remove(user);
        await this.databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// This async method get user's data by nickname from database.
    /// </summary>
    /// <param name="nickname">Uniqe user's nickname.</param>
    /// <returns>User's data from database.</returns>
    public async Task<User> GetUserByNicknameAsync(string nickname)
    {
        return await this.databaseContext.Users
            .Where(u => u.Nickname == nickname)
            .FirstOrDefaultAsync();
    }
}
