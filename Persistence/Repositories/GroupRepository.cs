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
    /// <param name="id">GRoup's id.</param>
    /// <returns>Group's data from database.</returns>
    public async Task<Group> GetGroupByIdAsync(int id)
    {
        return await this.databaseContext.Groups
            .Where(g => g.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Group> CreateGroupAsync()
    {
        Group group = new Group();

        // Generating joiningCode
        group.JoiningCode = await this.GenerateUniqueJoiningCodeAsync();

        // Adding group
        await this.databaseContext.Groups.AddAsync(group);

        await this.databaseContext.SaveChangesAsync();

        return group;
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
