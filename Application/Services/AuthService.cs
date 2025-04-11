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
        var existing = await userRepository.GetByEmailAsync(request.Email);

        if (existing is not null)
        {
            throw new EmailAlreadyUsedException();
        }

        var user = new User
        {
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

    /// <inheritdoc/>
    public async Task<AuthResponseDto> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request)
    {
        var user = await userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException(userId);
        var isPasswordValid = passwordHasher.Verify(user.PasswordHash, request.CurrentPassword);
        if (!isPasswordValid)
        {
            throw new AppException(400, "INVALID_CURRENT_PASSWORD", "Invalid password.");
        }

        user.PasswordHash = passwordHasher.Hash(request.NewPassword);
        await userRepository.UpdateAsync(user);
        return this.CreateAuthResponse(user);
    }

    private AuthResponseDto CreateAuthResponse(User user)
    {
        var token = jwtTokenService.GenerateToken(user);
        return new AuthResponseDto(user.Id, user.Nickname, user.Email, token);
    }
}
