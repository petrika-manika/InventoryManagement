namespace InventoryManagement.Application.Common.Interfaces;

/// <summary>
/// Interface for password hashing operations.
/// Provides methods to securely hash passwords and verify them.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password using a secure hashing algorithm.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>The hashed password.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies that a plain text password matches a previously hashed password.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="passwordHash">The hashed password to compare against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    bool VerifyPassword(string password, string passwordHash);
}
