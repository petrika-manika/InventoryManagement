namespace InventoryManagement.API.Tests.IntegrationTests;

/// <summary>
/// Result returned from authentication endpoint.
/// </summary>
public class AuthenticationResult
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
}

/// <summary>
/// User data transfer object.
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>
/// Result returned from stock operations (add/remove).
/// </summary>
public class StockOperationResult
{
    public Guid ProductId { get; set; }
    public int NewStockQuantity { get; set; }
}
