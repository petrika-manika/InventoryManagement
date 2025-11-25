namespace InventoryManagement.Application.Common.Models;

/// <summary>
/// Data Transfer Object for business client information.
/// Represents a company/organization customer.
/// </summary>
public sealed class BusinessClientDto : ClientDto
{
    /// <summary>
    /// Gets or sets the business NIPT (tax ID).
    /// </summary>
    public string NIPT { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the business owner's first name.
    /// </summary>
    public string? OwnerFirstName { get; set; }

    /// <summary>
    /// Gets or sets the business owner's last name.
    /// </summary>
    public string? OwnerLastName { get; set; }

    /// <summary>
    /// Gets or sets the business owner's phone number.
    /// </summary>
    public string? OwnerPhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the business owner's full name.
    /// </summary>
    public string? OwnerFullName { get; set; }

    /// <summary>
    /// Gets or sets the contact person's first name.
    /// </summary>
    public string ContactPersonFirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact person's last name.
    /// </summary>
    public string ContactPersonLastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact person's phone number.
    /// </summary>
    public string? ContactPersonPhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the contact person's full name.
    /// </summary>
    public string ContactPersonFullName { get; set; } = string.Empty;
}
