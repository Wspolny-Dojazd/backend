﻿using Domain.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines a contract for <see cref="Group"/> data access operations.
/// </summary>
public interface IGroupRepository
{
    /// <summary>
    /// Retrieves a group by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <returns>The group if found; otherwise, <see langword="null"/>.</returns>
    Task<Group?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a group by its unique identifier.
    /// </summary>
    /// <param name="code">The unique joining code of the group.</param>
    /// <returns>The group if found; otherwise, <see langword="null"/>.</returns>
    Task<Group?> GetByCodeAsync(string code);

    /// <summary>
    /// Adds a new group to the context.
    /// </summary>
    /// <param name="group">The group to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(Group group);

    /// <summary>
    /// Removes a group from the context.
    /// </summary>
    /// <param name="group">The group to remove.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveAsync(Group group);

    /// <summary>
    /// Adds a user to a group.
    /// </summary>
    /// <param name="group">The group to update.</param>
    /// <param name="user">The user to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddUserAsync(Group group, User user);

    /// <summary>
    /// Removes a user from a group.
    /// </summary>
    /// <param name="group">The group to update.</param>
    /// <param name="user">The user to remove.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveUserAsync(Group group, User user);

    /// <summary>
    /// Generates unique joinign code.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<string> GenerateUniqueJoiningCodeAsync();

    /// <summary>
    /// Retrieves all groups that the specified user is a member of.
    /// </summary>
    /// <param name="userId">The unique identifier of a user.</param>
    /// <returns>The list of groups the user is a member of.</returns>
    Task<List<Group>> GetGroupsByUserIdAsync(Guid userId);

    /// <summary>
    /// Determines whether the specified user is a member of the given group.
    /// </summary>
    /// <param name="groupId">The unique identifier of a group.</param>
    /// <param name="userId">The unique identifier of a user.</param>
    /// <returns>
    /// <see langword="true"/> if the user is a member of the group;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    Task<bool> HasMemberAsync(int groupId, Guid userId);

    /// <summary>
    /// Determines whether the specified user is an owner of the given group.
    /// </summary>
    /// <param name="groupId">The unique identifier of a group.</param>
    /// <param name="userId">The unique identifier of a user.</param>
    /// <returns>
    /// <see langword="true"/> if the user is an owner of the group;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    Task<bool> IsOwnerAsync(int groupId, Guid userId);
}
