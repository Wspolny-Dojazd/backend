using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<LoginUserReturnDto> LoginUserAsync(LoginUserDto userLoginDto);

    Task<bool> ValidateEmailAsync(string email);

    Task<LoginUserReturnDto> RegisterUserAsync(RegisterUserDto userRegisterDto);
}
