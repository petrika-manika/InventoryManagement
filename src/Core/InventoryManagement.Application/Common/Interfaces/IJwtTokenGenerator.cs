using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Common.Interfaces;

/// <summary>
/// Interface for JWT token generation.
/// Generates JSON Web Tokens for authenticated users.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a JWT token for an authenticated user.
    /// </summary>
    /// <param name="user">The user to generate the token for.</param>
    /// <returns>A JWT token string.</returns>
    string GenerateToken(User user);
}
