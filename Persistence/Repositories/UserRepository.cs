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
}
