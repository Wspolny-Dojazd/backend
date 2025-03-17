using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Persistence.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Application.Services;

public class AuthService
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordService passwordService;
    private readonly IJWTTokenService jwtTokenService;

    public AuthService(IUserRepository userRepository, IPasswordService passwordService, IJWTTokenService jwtTokenService)
    {
        this.userRepository = userRepository;
        this.passwordService = passwordService;
        this.jwtTokenService = jwtTokenService;
    }

    public async Task<LoginUserReturnDto> LoginUserAsync(LoginUserDto userLoginDto)
    {
        var user = await userRepository.GetUserByEmailAsync(userLoginDto.Email);

        if (user != null && passwordService.VerifyPassword(user.PasswordHash, userLoginDto.Password))
        {
            await userRepository.UpdateUserAsync(user);

            var token = jwtTokenService.GenerateToken(user);

            var userReturnData = new LoginUserReturnDto
            {
                Id = user.Id,
                Nickname = user.Nickname,
                JWTToken = token,
                Email = user.Email,
            };

            return userReturnData;
        }
        else
            return null;
    }

}
