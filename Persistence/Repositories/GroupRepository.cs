using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

/// <summary>
/// Represents crud operations for group.
/// </summary>
public class GroupRepository : IGroupRepository
{
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
    public async Task<Group?> GetGroupByIdAsync(int id)
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
    public async Task<Group?> GetGroupByCodeAsync(string code)
    {
        return await this.databaseContext.Groups
            .Include(g => g.GroupMembers)
            .Where(g => g.JoiningCode == code)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<Group> CreateGroupAsync()
    {
        Group group = new Group
        {
            JoiningCode = await this.GenerateUniqueJoiningCodeAsync(),
            Routes = new List<Route>(),
            LiveLocations = new List<Location>(),
            GroupMembers = new List<User>(),
        };

        // Adding group
        _ = await this.databaseContext.Groups.AddAsync(group);

        _ = await this.databaseContext.SaveChangesAsync();

        return group;
    }

    /// <inheritdoc/>
    public async void SaveAsync()
    {
        _ = await this.databaseContext.SaveChangesAsync();
    }

    private async Task<string> GenerateUniqueJoiningCodeAsync()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string joiningCode;

        bool isUnique;

        do
        {
            joiningCode = new string(Enumerable.Range(0, 6)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());

            isUnique = !await this.databaseContext.Groups.AnyAsync(g => g.JoiningCode == joiningCode);
        } while (!isUnique);

        return joiningCode;
    }
}
