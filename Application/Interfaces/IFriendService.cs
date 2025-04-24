using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for managing friend-related operations.
/// </summary>
public interface IFriendService
{
    /// <summary>
    /// Creates a new friendship between two users.
    /// </summary>
    /// <param name="userId">The unique identifier of the user initiating the friendship.</param>
    /// <param name="friendId">The unique identifier of the user to be added as a friend.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CreateFriendshipAsync(Guid userId, Guid friendId);

    /// <summary>
    /// Retrieves all friends of a specific user.
    /// </summary>
    /// <param name="userId">
    /// The unique identifier of the user whose friends are to be retrieved.
    /// </param>
    /// <returns>The friends of the specified user.</returns>
    Task<IEnumerable<UserDto>> GetFriendsAsync(Guid userId);

    /// <summary>
    /// Determines whether two users are already friends.
    /// </summary>
    /// <param name="userId">The unique identifier of the first user.</param>
    /// <param name="friendId">The unique identifier of the second user.</param>
    /// <returns>
    /// <see langword="true"/> if the users are friends;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    Task<bool> AreFriendsAsync(Guid userId, Guid friendId);
}
