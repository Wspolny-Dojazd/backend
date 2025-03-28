using System.Security.Cryptography;
using Application.Interfaces;

namespace Application.Services;

/// <summary>
/// Provides password hashing and verification.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 10000;
    private const char Delimiter = ';';
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA256;

    /// <inheritdoc/>
    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    /// <inheritdoc/>
    public bool Verify(string hashedPassword, string providedPassword)
    {
        var elements = hashedPassword.Split(Delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(providedPassword, salt, Iterations, HashAlgorithmName, KeySize);

        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}
