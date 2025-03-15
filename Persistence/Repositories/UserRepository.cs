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
}
