using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

/// <summary>
/// Represents authorization operations.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordService passwordService;
    private readonly IJWTTokenService jwtTokenService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="userRepository">User repository that allows database operations on user table.</param>
    /// <param name="passwordService">Represents repository that allows password operations.</param>
    /// <param name="jwtTokenService">Represents repository that allows generating JWT token for the user.</param>
    /// <param name="mapper">Used to map objects between different classes.</param>
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

    /// <summary>
    /// Method that logs in the user.
    /// </summary>
    /// <param name="userLoginDto">User's id, email, nickname and JWT token.</param>
    /// <returns>User's data.</returns>
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

    /// <summary>
    /// Method that registers a new user.
    /// </summary>
    /// <param name="userRegisterDto">User's id, email, nickname and JWT token.</param>
    /// <returns>New user's data.</returns>
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

    /// <summary>
    /// Method that validate user's email.
    /// </summary>
    /// <param name="email">User's email.</param>
    /// <returns>True if user with provided email exists and false if not.</returns>
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
