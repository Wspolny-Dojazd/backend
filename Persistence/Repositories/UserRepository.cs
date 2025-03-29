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
    public async Task<User?> GetByIdAsync(int id)
    {
        return await databaseContext.Users
            .Include(u => u.Groups)
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await databaseContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <inheritdoc/>
    public async Task AddAsync(User user)
    {
        _ = await databaseContext.Users.AddAsync(user);
        _ = await databaseContext.SaveChangesAsync();
    }
}
