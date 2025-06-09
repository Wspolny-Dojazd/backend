using Application.Services;
using Domain.Model;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Test;

[TestFixture]
public class JWTTokenServiceTests
{
    private JWTTokenService _tokenService;
    private IConfiguration _configuration;

    [SetUp]
    public void SetUp()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "test_secret_key_1234567890_ABCDEF!" }, // 32+ chars
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings!)
            .Build();

        _tokenService = new JWTTokenService(_configuration);
    }

    [Test]
    public void GenerateToken_WithValidUser_ReturnsValidJwt()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            Nickname = "Tester",
            PasswordHash = "hashed_pw",
            RefreshToken = "token",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
            Friends = [],
            Groups = [],
            UserConfiguration = new UserConfiguration()
        };

        // Act
        var token = _tokenService.GenerateToken(user);

        // Assert
        Assert.That(token, Is.Not.Null.And.Not.Empty);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var usernameClaim = jwt.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

        Assert.That(usernameClaim, Is.EqualTo(user.Username));
    }
}
