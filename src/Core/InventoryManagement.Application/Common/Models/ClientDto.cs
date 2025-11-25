namespace InventoryManagement.Application.Common.Models;

/// <summary>
/// Base Data Transfer Object for client information.
/// Contains common properties shared across all client types.
/// </summary>
public class ClientDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the client.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client type as a string (enum name).
    /// </summary>
    public string ClientType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client type enum value.
    /// </summary>
    public int ClientTypeId { get; set; }

    /// <summary>
    /// Gets or sets the client's address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the client's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the client's phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the client.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the client was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the client was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user who created this client.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the user who last updated this client.
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the client is active.
    /// </summary>
    public bool IsActive { get; set; }
}
