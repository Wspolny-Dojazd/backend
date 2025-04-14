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
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await databaseContext.Users
            .Include(u => u.Friends)
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
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await databaseContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    /// <inheritdoc/>
    public async Task AddAsync(User user)
    {
        _ = await databaseContext.Users.AddAsync(user);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(User user)
    {
        _ = databaseContext.Users.Update(user);
        _ = await databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// Creates a friendship relationship between two users.
    /// </summary>
    /// <param name="userId1">The ID of the first user in the friendship.</param>
    /// <param name="userId2">The ID of the second user in the friendship.</param>
    /// <returns>A task representing the asynchronous operation of adding the users as friends.</returns>
    /// <remarks>
    /// This method establishes a bidirectional friendship relationship,
    /// meaning both users will appear in each other's friends list.
    /// If either user doesn't exist, the operation silently returns without creating any relationship.
    /// </remarks>
    public async Task AddFriendAsync(Guid userId1, Guid userId2)
    {
        var user1 = await databaseContext.Users
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == userId1);

        var user2 = await databaseContext.Users
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == userId2);

        if (user1 == null || user2 == null)
        {
            return;
        }

        user1.Friends.Add(user2);
        user2.Friends.Add(user1);

        await databaseContext.SaveChangesAsync();
    }
}
