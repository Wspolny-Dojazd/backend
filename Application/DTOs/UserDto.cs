namespace Application.DTOs;

/// <summary>
/// Represents the data transfer object used for returning user data in API responses.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Nickname">The nickname of the user.</param>
/// <param name="Email">The email address of the user.</param>
public record UserDto(int Id, string Nickname, string Email);
