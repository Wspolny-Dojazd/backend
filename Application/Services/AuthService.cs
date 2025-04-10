using System.Text.RegularExpressions;
using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

/// <summary>
/// Provides authentication operations.
/// </summary>
/// <param name="userRepository">The repository for accessing user data.</param>
/// <param name="jwtTokenService">The JWT token service.</param
/// <param name="passwordHasher">The password hashing service.</param>
public class AuthService(
    IUserRepository userRepository,
    IJWTTokenService jwtTokenService,
    IPasswordHasher passwordHasher)
    : IAuthService
{
    /// <inheritdoc/>
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email)
            ?? throw new InvalidCredentialsException();

        return passwordHasher.Verify(user.PasswordHash, request.Password)
            ? this.CreateAuthResponse(user)
            : throw new InvalidCredentialsException();
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

        var isValidUsername = request.Username.Length < 3 || request.Username.Length > 32 || !Regex.IsMatch(request.Username, "^[a-z0-9_]+$");

        if (isValidUsername)
        {
            throw new AppException(400, "USERNAME_VALIDATION_ERROR", "Username must be between 3 and 32 characters and can only contain lowercase letters, numbers, and underscores.");
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
        };

        await userRepository.AddAsync(user);
        return this.CreateAuthResponse(user);
    }

    /// <inheritdoc/>
    public async Task<AuthResponseDto> GetMeAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException(userId);

        return this.CreateAuthResponse(user);
    }

    private AuthResponseDto CreateAuthResponse(User user)
    {
        var token = jwtTokenService.GenerateToken(user);
        return new AuthResponseDto(user.Id, user.Username, user.Nickname, user.Email, token);
    }
}
