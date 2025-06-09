using Moq;
using Application.Services;
using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using AutoMapper;

namespace Test;

[TestFixture]
public class AuthServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IJWTTokenService> _jwtTokenServiceMock;
    private Mock<IPasswordHasher> _passwordHasherMock;
    private Mock<IMapper> _mapperMock;
    private AuthService _authService;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtTokenServiceMock = new Mock<IJWTTokenService>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _mapperMock = new Mock<IMapper>();

        _authService = new AuthService(
            _userRepositoryMock.Object,
            _jwtTokenServiceMock.Object,
            _passwordHasherMock.Object,
            _mapperMock.Object
        );
    }

    [Test]
    public async Task LoginAsync_WithValidCredentials_ReturnsAuthResponse()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Nickname = "tester",
            Email = "test@example.com",
            PasswordHash = "hashed_password",
            RefreshToken = "refresh-token",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5),
            Friends = [],
            Groups = [],
            UserConfiguration = new UserConfiguration()
        };

        var request = new LoginRequestDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.Verify(user.PasswordHash, request.Password)).Returns(true);
        _jwtTokenServiceMock.Setup(j => j.GenerateToken(user)).Returns("fake-token");

        var result = await _authService.LoginAsync(request);

        Assert.That(result.Id, Is.EqualTo(user.Id));
        Assert.That(result.Username, Is.EqualTo(user.Username));
        Assert.That(result.Nickname, Is.EqualTo(user.Nickname));
        Assert.That(result.Email, Is.EqualTo(user.Email));
        Assert.That(result.Token, Is.EqualTo("fake-token"));
        Assert.That(result.RefreshToken, Is.Not.Null);
    }

    [Test]
    public void LoginAsync_WithInvalidPassword_ThrowsInvalidCredentialsException()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Nickname = "tester",
            Email = "test@example.com",
            PasswordHash = "hashed_password",
            RefreshToken = "refresh-token",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5),
            Friends = [],
            Groups = [],
            UserConfiguration = new UserConfiguration()
        };

        var request = new LoginRequestDto
        {
            Email = "test@example.com",
            Password = "wrong-password"
        };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.Verify(user.PasswordHash, request.Password)).Returns(false);

        Assert.ThrowsAsync<InvalidCredentialsException>(() => _authService.LoginAsync(request));
    }

    [Test]
    public async Task RegisterAsync_WithValidData_AddsUserAndReturnsAuthResponse()
    {
        var request = new RegisterRequestDto
        {
            Username = "newuser",
            Email = "newuser@example.com",
            Nickname = "nick",
            Password = "secure-password"
        };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(request.Username)).ReturnsAsync((User?)null);
        _passwordHasherMock.Setup(h => h.Hash(request.Password)).Returns("hashed_pw");
        _jwtTokenServiceMock.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns("token");

        User addedUser = null!;
        _userRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => addedUser = u)
            .Returns(Task.CompletedTask);

        var result = await _authService.RegisterAsync(request);

        Assert.That(addedUser, Is.Not.Null);
        Assert.That(addedUser.Username, Is.EqualTo(request.Username));
        Assert.That(addedUser.Email, Is.EqualTo(request.Email));
        Assert.That(addedUser.PasswordHash, Is.EqualTo("hashed_pw"));

        Assert.That(result.Username, Is.EqualTo(request.Username));
        Assert.That(result.Email, Is.EqualTo(request.Email));
        Assert.That(result.Token, Is.EqualTo("token"));
        Assert.That(result.RefreshToken, Is.Not.Null);
    }
}
