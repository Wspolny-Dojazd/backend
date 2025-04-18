using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for group-related operations.
/// </summary>
public interface IGroupService
{
    /// <summary>
    /// Retrieves a group by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <returns>The requested group details.</returns>
    Task<GroupDto> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new group.
    /// </summary>
    /// <param name="creatorId">The unique identifier of the group creator.</param>
    /// <returns>The created group details.</returns>
    Task<GroupDto> CreateAsync(Guid creatorId);

    /// <summary>
    /// Adds the specified user to the group using a joining code.
    /// </summary>
    /// <param name="joiningCode">The unique joining code of the group.</param>
    /// <param name="userId">The unique identifier of the user to add.</param>
    /// <returns>The updated group details.</returns>
    Task<GroupDto> AddUserByCodeAsync(string joiningCode, Guid userId);

    /// <summary>
    /// Removes the specified user from the group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user to remove.</param>
    /// <returns>
    /// The updated group details if the user was removed;
    /// <see langword = "null" /> if the user was the group creator and the group was deleted.
    /// </returns>
    Task<GroupDto?> RemoveUserAsync(int groupId, Guid userId);

    /// <summary>
    /// Removes the specified user from the group, unless they are the group's creator.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user to remove.</param>
    /// <returns>The updated group details.</returns>
    Task<GroupDto> KickUserAsync(int groupId, Guid userId);

    /// <summary>
    /// Retrieves all groups that the specified user is a member of.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A collection of groups that the user belongs to.</returns>
    Task<IEnumerable<GroupDto>> GetGroupsForUserAsync(Guid userId);

    /// <summary>
    /// Retrieves all members of the specified group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>A collection representing users who are members of the specified group.</returns>
    Task<IEnumerable<GroupMemberDto>> GetGroupMembersAsync(int groupId);
}
