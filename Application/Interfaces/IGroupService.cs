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
    /// <returns>The group if found; otherwise, <see langword="null"/>.</returns>
    Task<GroupDto> GetGroupByIdAsync(int id);

    /// <summary>
    /// Creates new group.
    /// </summary>
    /// <returns>The group if created; otherwise, <see langword="null"/>.</returns>
    Task<GroupDto> CreateGroupAsync();

    /// <summary>
    /// Adds the user to the group.
    /// </summary>
    /// <param name="code">The unique joining code to the group.</param>
    /// <param name="userId">The unique identifier of the group.</param>
    /// <returns>The group if user joined; otherwise, <see langword="null"/>.</returns>
    Task<GroupDto> AddUserViaCodeAsync(string code, int userId);

    /// <summary>
    /// Removes the user from the group.
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The user if found; otherwise, <see langword="null"/>.</returns>
    Task<GroupDto> RemoveUserFromGroupAsync(int id, int userId);
}
