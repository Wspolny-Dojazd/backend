using Domain.Model;

namespace Domain.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Method that gets user display data.
    /// </summary>
    /// <param name="id">Unique user identifier.</param>
    /// <returns>Returns user display data.</returns>
    Task<User> GetUserByIdAsync(int id);

    /// <summary>
    /// Method that gets all users display data.
    /// </summary>
    /// <returns>Returns all users display data.</returns>
    Task<List<User>> GetAllUsersAsync();

    /// <summary>
    /// Method that deletes the user's data.
    /// </summary>
    /// <param name="user">user data.</param>
    Task DeleteUserAsync(User user);

    /// <summary>
    /// This async method get user's data by id from database.
    /// </summary>
    /// <param name="nickname">Uniqe user's nickname.</param>
    /// <returns>User's data from database.</returns>
    Task<User> GetUserByNicknameAsync(string nickname);
}
