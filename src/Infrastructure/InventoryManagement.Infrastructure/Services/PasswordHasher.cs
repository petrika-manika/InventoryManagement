using InventoryManagement.Application.Common.Interfaces;

namespace InventoryManagement.Infrastructure.Services;

/// <summary>
/// Password hashing service using BCrypt algorithm.
/// BCrypt is an industry-standard password hashing algorithm that is secure and 
/// intentionally slow to prevent brute-force attacks.
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password using BCrypt algorithm.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>The hashed password with salt.</returns>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifies a plain text password against a BCrypt hash.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="passwordHash">The BCrypt hash to compare against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
