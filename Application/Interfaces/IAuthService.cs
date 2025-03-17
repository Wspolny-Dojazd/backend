using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<LoginUserReturnDto> LoginUserAsync(LoginUserDto userLoginDto);
}
