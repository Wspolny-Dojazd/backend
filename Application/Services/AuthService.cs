using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Persistence.Repositories;

namespace Application.Services;

/// <summary>
///
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordService passwordService;
    private readonly IJWTTokenService jwtTokenService;
    private readonly IMapper mapper;

    public AuthService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IJWTTokenService jwtTokenService,
        IMapper mapper)
    {
        this.userRepository = userRepository;
        this.passwordService = passwordService;
        this.jwtTokenService = jwtTokenService;
        this.mapper = mapper;
    }

    public async Task<LoginUserReturnDto> LoginUserAsync(LoginUserDto userLoginDto)
    {
        var user = await this.userRepository.GetUserByEmailAsync(userLoginDto.Email);

        if (user != null && this.passwordService.VerifyPassword(user.PasswordHash, userLoginDto.Password))
        {
            await this.userRepository.UpdateUserAsync(user);

            var token = this.jwtTokenService.GenerateToken(user);

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
        {
            return null;
        }
    }

    public async Task<LoginUserReturnDto> RegisterUserAsync(RegisterUserDto userRegisterDto)
    {
        var user = this.mapper.Map<RegisterUserDto, User>(userRegisterDto);
        user.Nickname = user.Nickname.ToLower();
        user.PasswordHash = this.passwordService.HashPassword(userRegisterDto.Password);
        user.CreatedAt = DateTime.Now;
        await this.userRepository.InsertUserAsync(user);

        var token = this.jwtTokenService.GenerateToken(user);

        var userReturnData = new LoginUserReturnDto
        {
            Id = user.Id,
            Email = user.Email,
            Nickname = user.Nickname,
            JWTToken = token,
        };

        return userReturnData;
    }

    public async Task<bool> ValidateEmailAsync(string email)
    {
        var user = await this.userRepository.GetUserByEmailAsync(email);

        if (user == null)
        {
            return true;
        }

        return false;
    }
}
