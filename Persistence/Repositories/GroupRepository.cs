using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Represents crud operations for group.
/// </summary>
public class GroupRepository : IGroupRepository
{
    private static readonly Random Random = new();
    private readonly DatabaseContext databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupRepository"/> class.
    /// </summary>
    /// <param name="databaseContext">Database context that allows to get and manipulate database data.</param>
    public GroupRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    /// <summary>
    /// This async method get group's data by id from database.
    /// </summary>
    /// <param name="id">Group's id.</param>
    /// <returns>Group's data from database.</returns>
    public async Task<Group?> GetByIdAsync(int id)
    {
        return await this.databaseContext.Groups
            .Include(g => g.GroupMembers)
            .Where(g => g.Id == id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// This async method get group's data by id from database.
    /// </summary>
    /// <param name="code">Group's joining code.</param>
    /// <returns>Group's data from database.</returns>
    public async Task<Group?> GetByCodeAsync(string code)
    {
        return await this.databaseContext.Groups
            .Include(g => g.GroupMembers)
            .Where(g => g.JoiningCode == code)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task AddAsync(Group group)
    {
        _ = await this.databaseContext.Groups.AddAsync(group);
        _ = await this.databaseContext.SaveChangesAsync();

        return;
    }

    /// <inheritdoc/>
    public async Task AddUserAsync(Group group, User user)
    {
        group.GroupMembers.Add(user);
        _ = await this.databaseContext.SaveChangesAsync();
        return;
    }

    /// <inheritdoc/>
    public async Task RemoveUserAsync(Group group, User user)
    {
        _ = group.GroupMembers.Remove(user);
        _ = await this.databaseContext.SaveChangesAsync();
        return;
    }

    /// <inheritdoc/>
    public async Task<string> GenerateUniqueJoiningCodeAsync()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string joiningCode;

        bool isUnique;

        do
        {
            joiningCode = new string(Enumerable.Range(0, 6)
                .Select(_ => chars[Random.Next(chars.Length)])
                .ToArray());

            isUnique = !await this.databaseContext.Groups.AnyAsync(g => g.JoiningCode == joiningCode);
        } while (!isUnique);

        return joiningCode;
    }
}
