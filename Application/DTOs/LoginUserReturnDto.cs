namespace Application.DTOs;

public class LoginUserReturnDto
{
    public required int Id { get; set; }

    public required string Email { get; set; }

    public required string Nickname { get; set; }

    public required string JWTToken { get; set; }
}
