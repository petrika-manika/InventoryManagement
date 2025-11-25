namespace InventoryManagement.Application.Common.Models;

/// <summary>
/// Data Transfer Object for authentication response.
/// Contains user information and JWT token for authenticated users.
/// </summary>
public sealed class AuthenticationResult
{
    /// <summary>
    /// Gets or sets the authenticated user's information.
    /// </summary>
    public UserDto User { get; set; } = null!;

    /// <summary>
    /// Gets or sets the JWT authentication token.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}
