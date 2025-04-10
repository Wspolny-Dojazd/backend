using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="GroupMember"/> data access operations.
/// </summary>
public interface IGroupMemberRepository
{
    /// <summary>
    /// Retrieves all members of a group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>A collection of group members.</returns>
    Task<List<GroupMember>> GetGroupMembersAsync(int groupId);

    /// <summary>
    /// Retrieves a group member by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>The group member if found; otherwise, <see langword="null"/>.</returns>
    Task<GroupMember?> GetGroupMemberbyIdAsync(Guid userId, int groupId);
}
