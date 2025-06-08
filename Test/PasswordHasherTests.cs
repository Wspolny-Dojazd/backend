using Application.Services;

namespace Test;

[TestFixture]
public class PasswordHasherTests
{
    private PasswordHasher _hasher;

    [SetUp]
    public void SetUp()
    {
        _hasher = new PasswordHasher();
    }

    [Test]
    public void Hash_WhenCalled_ReturnsNonEmptyHash()
    {
        // Arrange
        var password = "secret123";

        // Act
        var hash = _hasher.Hash(password);

        // Assert
        Assert.That(hash, Is.Not.Null.And.Not.Empty);
        Assert.That(hash, Is.Not.EqualTo(password)); // shouldn't be plain
    }

    [Test]
    public void Verify_WithCorrectPassword_ReturnsTrue()
    {
        // Arrange
        var password = "admin123";
        var hash = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify(hash, password);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Verify_WithIncorrectPassword_ReturnsFalse()
    {
        // Arrange
        var correctPassword = "admin123";
        var wrongPassword = "admin999";
        var hash = _hasher.Hash(correctPassword);

        // Act
        var result = _hasher.Verify(hash, wrongPassword);

        // Assert
        Assert.That(result, Is.False);
    }
}
