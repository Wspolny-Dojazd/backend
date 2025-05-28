namespace Application.Interfaces;

/// <summary>
/// Defines a contract for authentication operations.
/// </summary>
public interface IGroupAuthorizationService
{
    /// <summary>
    /// Ensures that the user is a member of the given group.
    /// </summary>
    /// <param name="groupId"><The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task EnsureMembershipAsync(int groupId, Guid userId);

    /// <summary>
    /// Ensures that the user is an owner of the given group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task EnsureOwnershipAsync(int groupId, Guid userId);
}
