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
        var password = "secret123";

        var hash = _hasher.Hash(password);

        Assert.That(hash, Is.Not.Null.And.Not.Empty);
        Assert.That(hash, Is.Not.EqualTo(password));
    }

    [Test]
    public void Verify_WithCorrectPassword_ReturnsTrue()
    {
        var password = "admin123";
        var hash = _hasher.Hash(password);

        var result = _hasher.Verify(hash, password);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Verify_WithIncorrectPassword_ReturnsFalse()
    {
        var correctPassword = "admin123";
        var wrongPassword = "admin999";
        var hash = _hasher.Hash(correctPassword);

        var result = _hasher.Verify(hash, wrongPassword);

        Assert.That(result, Is.False);
    }
}
