using System.Security.Cryptography;
using Application.Constants;
using Application.DTOs;
using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

/// <summary>
/// Provides authentication operations.
/// </summary>
/// <param name="userRepository">The repository for accessing user data.</param>
/// <param name="jwtTokenService">The JWT token service.</param
/// <param name="passwordHasher">The password hashing service.</param>
/// <param name="mapper">The object mapper.</param>
public class AuthService(
    IUserRepository userRepository,
    IJWTTokenService jwtTokenService,
    IPasswordHasher passwordHasher,
    IMapper mapper)
    : IAuthService
{
    private const int RefreshTokenExpiryDays = 28;
    private const int RefreshTokenByteSize = 64;

    /// <inheritdoc/>
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email)
            ?? throw new InvalidCredentialsException();

        if (!passwordHasher.Verify(user.PasswordHash, request.Password))
        {
            throw new InvalidCredentialsException();
        }

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays);

        await userRepository.UpdateAsync(user);

        var token = jwtTokenService.GenerateToken(user);
        return new AuthResponseDto(user.Id, user.Username, user.Nickname, user.Email, token, user.RefreshToken);
    }

    /// <inheritdoc/>
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingEmail = await userRepository.GetByEmailAsync(request.Email);
        if (existingEmail is not null)
        {
            throw new EmailAlreadyUsedException();
        }

        var existingUsername = await userRepository.GetByUsernameAsync(request.Username);
        if (existingUsername is not null)
        {
            throw new AppException(409, "USERNAME_ALREADY_USED", "Username is already used.");
        }

        if (ReservedUsernames.List.Contains(request.Username))
        {
            throw new AppException(400, "USERNAME_RESERVED", "This username is reserved.");
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Nickname = request.Nickname,
            PasswordHash = passwordHasher.Hash(request.Password),
            CreatedAt = DateTime.UtcNow,
            Friends = [],
            Groups = [],
            UserConfiguration = new UserConfiguration(),
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays),
        };

        await userRepository.AddAsync(user);

        var token = jwtTokenService.GenerateToken(user);
        return new AuthResponseDto(user.Id, user.Username, user.Nickname, user.Email, token, user.RefreshToken);
    }

    /// <inheritdoc/>
    public async Task<UserDto> GetMeAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException(userId);

        return mapper.Map<UserDto>(user);
    }

    /// <inheritdoc/>
    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var principal = jwtTokenService.GetPrincipalFromExpiredToken(request.Token);
        var username = principal?.Claims?.FirstOrDefault(c => c.Type == "username")?.Value
            ?? throw new AppException(400, "INVALID_TOKEN");

        var user = await userRepository.GetByUsernameAsync(username)
            ?? throw new UserNotFoundException(username);

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow || user.RefreshToken != request.RefreshToken)
        {
            throw new AppException(400, "INVALID_REFRESH_TOKEN");
        }

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays);

        await userRepository.UpdateAsync(user);

        var token = jwtTokenService.GenerateToken(user);
        return new AuthResponseDto(user.Id, user.Username, user.Nickname, user.Email, token, user.RefreshToken);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[RefreshTokenByteSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
