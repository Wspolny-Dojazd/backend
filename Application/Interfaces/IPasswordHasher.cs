namespace Application.Interfaces;

/// <summary>
/// Defines a contract for password hashing operations.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a password using a secure algorithm.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    string Hash(string password);

    /// <summary>
    /// Verifies whether a given password matches a stored hash.
    /// </summary>
    /// <param name="hash">The stored hash.</param>
    /// <param name="password">The password to verify.</param>
    /// <returns>
    /// <see langword="true"/> if the password matches the hash;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool Verify(string hash, string password);
}
