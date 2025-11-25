namespace InventoryManagement.Application.Common.Interfaces;

/// <summary>
/// Interface to get current authenticated user information from HTTP context.
/// Provides access to the currently authenticated user's details.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the ID of the current authenticated user.
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// Gets the email of the current authenticated user.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Gets a value indicating whether the current user is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }
}
