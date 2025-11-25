namespace InventoryManagement.Application.Common.Models;

/// <summary>
/// Data Transfer Object for individual client information.
/// Represents a person/individual customer.
/// </summary>
public sealed class IndividualClientDto : ClientDto
{
    /// <summary>
    /// Gets or sets the client's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client's full name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}
