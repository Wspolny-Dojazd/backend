namespace Application.DTOs;

/// <summary>
/// Represents user's data returned after logging in.
/// </summary>
public class LoginUserReturnDto
{
    /// <summary>
    /// Gets or sets user's id.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Gets or sets user's email.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets user's nickname.
    /// </summary>
    public required string Nickname { get; set; }

    /// <summary>
    /// Gets or sets user's JWT Token.
    /// </summary>
    public required string JWTToken { get; set; }
}
