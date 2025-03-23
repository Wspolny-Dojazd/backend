using System.Security.Cryptography;
using Application.Interfaces;

namespace Application.Services;

/// <summary>
/// Represents password operations.
/// </summary>
public class PasswordService : IPasswordService
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 10000;
    private const char Delimiter = ';';
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA256;

    /// <summary>
    /// Method that hashes the user's password.
    /// </summary>
    /// <param name="password">User's password that will be hashed.</param>
    /// <returns>Returns hashed user's password.</returns>
    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    /// <summary>
    /// Method that verifies if the hashed user's password and provided password match.
    /// </summary>
    /// <param name="hashedPassword">Hashed user's password.</param>
    /// <param name="providedPassword">Provided password.</param>
    /// <returns>Bool value depending on password match.</returns>
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var elements = hashedPassword.Split(Delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(providedPassword, salt, Iterations, HashAlgorithmName, KeySize);

        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}
