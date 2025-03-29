﻿using Application.DTOs;

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
    /// Creates new group.
    /// </summary>
    /// <returns>The created group details.</returns>
    Task<GroupDto> CreateAsync();

    /// <summary>
    /// Adds the specified user to the group using a joining code.
    /// </summary>
    /// <param name="joiningCode">The unique joining code of the group.</param>
    /// <param name="userId">The unique identifier of the group.</param>
    /// <returns>The updated group details.</returns>
    Task<GroupDto> AddUserByCodeAsync(string joiningCode, int userId);

    /// <summary>
    /// Removes the specified user from the group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The updated group details.</returns>
    Task<GroupDto> RemoveUserAsync(int groupId, int userId);
}
