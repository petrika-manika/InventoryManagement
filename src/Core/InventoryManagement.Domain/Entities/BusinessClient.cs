using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

/// <summary>
/// Represents a business client (company/organization) in the system.
/// </summary>
public sealed class BusinessClient : Client
{
    /// <summary>
    /// Gets the business NIPT (tax ID).
    /// </summary>
    public NIPT NIPT { get; private set; } = null!;

    /// <summary>
    /// Gets the business owner's first name.
    /// </summary>
    public string? OwnerFirstName { get; private set; }

    /// <summary>
    /// Gets the business owner's last name.
    /// </summary>
    public string? OwnerLastName { get; private set; }

    /// <summary>
    /// Gets the business owner's phone number.
    /// </summary>
    public string? OwnerPhoneNumber { get; private set; }

    /// <summary>
    /// Gets the contact person's first name.
    /// </summary>
    public string ContactPersonFirstName { get; private set; } = null!;

    /// <summary>
    /// Gets the contact person's last name.
    /// </summary>
    public string ContactPersonLastName { get; private set; } = null!;

    /// <summary>
    /// Gets the contact person's phone number.
    /// </summary>
    public string? ContactPersonPhoneNumber { get; private set; }

    /// <summary>
    /// Gets the business owner's full name.
    /// </summary>
    public string? OwnerFullName =>
        string.IsNullOrWhiteSpace(OwnerFirstName) || string.IsNullOrWhiteSpace(OwnerLastName)
            ? null
            : $"{OwnerFirstName} {OwnerLastName}";

    /// <summary>
    /// Gets the contact person's full name.
    /// </summary>
    public string ContactPersonFullName => $"{ContactPersonFirstName} {ContactPersonLastName}";

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    private BusinessClient()
    {
        // Required by EF Core for materialization
    }

    /// <summary>
    /// Creates a new business client.
    /// </summary>
    /// <param name="nipt">The business NIPT (tax ID).</param>
    /// <param name="contactPersonFirstName">The contact person's first name.</param>
    /// <param name="contactPersonLastName">The contact person's last name.</param>
    /// <param name="ownerFirstName">The owner's first name.</param>
    /// <param name="ownerLastName">The owner's last name.</param>
    /// <param name="ownerPhoneNumber">The owner's phone number.</param>
    /// <param name="contactPersonPhoneNumber">The contact person's phone number.</param>
    /// <param name="address">The business address.</param>
    /// <param name="email">The business email address.</param>
    /// <param name="phoneNumber">The business phone number.</param>
    /// <param name="notes">Additional notes about the client.</param>
    /// <param name="createdBy">The ID of the user creating this client.</param>
    /// <returns>A new BusinessClient instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when NIPT is null.</exception>
    /// <exception cref="InvalidClientDataException">Thrown when validation fails.</exception>
    public static BusinessClient Create(
        NIPT nipt,
        string contactPersonFirstName,
        string contactPersonLastName,
        string? ownerFirstName,
        string? ownerLastName,
        string? ownerPhoneNumber,
        string? contactPersonPhoneNumber,
        string? address,
        string? email,
        string? phoneNumber,
        string? notes,
        string createdBy)
    {
        if (nipt == null)
        {
            throw new ArgumentNullException(nameof(nipt), "NIPT cannot be null.");
        }

        var client = new BusinessClient
        {
            Id = Guid.NewGuid().ToString(),
            ClientType = ClientType.Business,
            NIPT = nipt,
            ContactPersonFirstName = ValidateAndTrimName(contactPersonFirstName, nameof(contactPersonFirstName)),
            ContactPersonLastName = ValidateAndTrimName(contactPersonLastName, nameof(contactPersonLastName)),
            OwnerFirstName = ValidateOptionalName(ownerFirstName, nameof(ownerFirstName)),
            OwnerLastName = ValidateOptionalName(ownerLastName, nameof(ownerLastName)),
            OwnerPhoneNumber = ownerPhoneNumber,
            ContactPersonPhoneNumber = contactPersonPhoneNumber,
            Address = address,
            Email = email,
            PhoneNumber = phoneNumber,
            Notes = notes,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Validate all phone numbers
        client.ValidateEmail();
        client.ValidatePhoneNumber();
        client.ValidateOwnerPhoneNumber();
        client.ValidateContactPersonPhoneNumber();

        return client;
    }

    /// <summary>
    /// Updates the business information.
    /// </summary>
    /// <param name="nipt">The new NIPT.</param>
    /// <param name="ownerFirstName">The new owner first name.</param>
    /// <param name="ownerLastName">The new owner last name.</param>
    /// <param name="ownerPhoneNumber">The new owner phone number.</param>
    /// <param name="contactPersonFirstName">The new contact person first name.</param>
    /// <param name="contactPersonLastName">The new contact person last name.</param>
    /// <param name="contactPersonPhoneNumber">The new contact person phone number.</param>
    /// <param name="address">The new address.</param>
    /// <param name="email">The new email address.</param>
    /// <param name="phoneNumber">The new phone number.</param>
    /// <param name="notes">The new notes.</param>
    /// <exception cref="ArgumentNullException">Thrown when NIPT is null.</exception>
    /// <exception cref="InvalidClientDataException">Thrown when validation fails.</exception>
    public void UpdateBusinessInfo(
        NIPT nipt,
        string? ownerFirstName,
        string? ownerLastName,
        string? ownerPhoneNumber,
        string contactPersonFirstName,
        string contactPersonLastName,
        string? contactPersonPhoneNumber,
        string? address,
        string? email,
        string? phoneNumber,
        string? notes)
    {
        if (nipt == null)
        {
            throw new ArgumentNullException(nameof(nipt), "NIPT cannot be null.");
        }

        // Validate names
        var validatedContactFirstName = ValidateAndTrimName(contactPersonFirstName, nameof(contactPersonFirstName));
        var validatedContactLastName = ValidateAndTrimName(contactPersonLastName, nameof(contactPersonLastName));
        var validatedOwnerFirstName = ValidateOptionalName(ownerFirstName, nameof(ownerFirstName));
        var validatedOwnerLastName = ValidateOptionalName(ownerLastName, nameof(ownerLastName));

        // Temporarily store phone numbers for validation
        var tempOwnerPhone = OwnerPhoneNumber;
        var tempContactPhone = ContactPersonPhoneNumber;

        OwnerPhoneNumber = ownerPhoneNumber;
        ContactPersonPhoneNumber = contactPersonPhoneNumber;

        try
        {
            ValidateOwnerPhoneNumber();
            ValidateContactPersonPhoneNumber();
        }
        catch
        {
            // Restore original values if validation fails
            OwnerPhoneNumber = tempOwnerPhone;
            ContactPersonPhoneNumber = tempContactPhone;
            throw;
        }

        // Update common fields (this will validate email and main phone)
        UpdateCommonFields(address, email, phoneNumber, notes);

        // Update business-specific properties
        NIPT = nipt;
        OwnerFirstName = validatedOwnerFirstName;
        OwnerLastName = validatedOwnerLastName;
        ContactPersonFirstName = validatedContactFirstName;
        ContactPersonLastName = validatedContactLastName;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the owner's phone number format.
    /// </summary>
    /// <exception cref="InvalidClientDataException">Thrown when the phone number format is invalid.</exception>
    private void ValidateOwnerPhoneNumber()
    {
        if (string.IsNullOrWhiteSpace(OwnerPhoneNumber))
        {
            return; // Optional
        }

        if (OwnerPhoneNumber.Length > 20)
        {
            throw new InvalidClientDataException($"Owner phone number cannot exceed 20 characters.");
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(OwnerPhoneNumber, @"^[\d\s\+\-\(\)]+$"))
        {
            throw new InvalidClientDataException(
                $"Invalid owner phone number format: '{OwnerPhoneNumber}'. Only digits, spaces, +, -, (, ) are allowed.");
        }
    }

    /// <summary>
    /// Validates the contact person's phone number format.
    /// </summary>
    /// <exception cref="InvalidClientDataException">Thrown when the phone number format is invalid.</exception>
    private void ValidateContactPersonPhoneNumber()
    {
        if (string.IsNullOrWhiteSpace(ContactPersonPhoneNumber))
        {
            return; // Optional
        }

        if (ContactPersonPhoneNumber.Length > 20)
        {
            throw new InvalidClientDataException($"Contact person phone number cannot exceed 20 characters.");
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(ContactPersonPhoneNumber, @"^[\d\s\+\-\(\)]+$"))
        {
            throw new InvalidClientDataException(
                $"Invalid contact person phone number format: '{ContactPersonPhoneNumber}'. Only digits, spaces, +, -, (, ) are allowed.");
        }
    }

    /// <summary>
    /// Validates and trims a required name field.
    /// </summary>
    /// <param name="name">The name to validate.</param>
    /// <param name="paramName">The parameter name for error messages.</param>
    /// <returns>The trimmed and validated name.</returns>
    /// <exception cref="InvalidClientDataException">Thrown when validation fails.</exception>
    private static string ValidateAndTrimName(string name, string paramName)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidClientDataException(
                $"{paramName} cannot be null, empty, or whitespace.");
        }

        var trimmedName = name.Trim();

        if (trimmedName.Length < 1 || trimmedName.Length > 50)
        {
            throw new InvalidClientDataException(
                $"{paramName} must be between 1 and 50 characters. Current length: {trimmedName.Length}.");
        }

        return trimmedName;
    }

    /// <summary>
    /// Validates and trims an optional name field.
    /// </summary>
    /// <param name="name">The name to validate.</param>
    /// <param name="paramName">The parameter name for error messages.</param>
    /// <returns>The trimmed and validated name, or null if not provided.</returns>
    /// <exception cref="InvalidClientDataException">Thrown when validation fails.</exception>
    private static string? ValidateOptionalName(string? name, string paramName)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null; // Optional field
        }

        var trimmedName = name.Trim();

        if (trimmedName.Length < 1 || trimmedName.Length > 50)
        {
            throw new InvalidClientDataException(
                $"{paramName} must be between 1 and 50 characters when provided. Current length: {trimmedName.Length}.");
        }

        return trimmedName;
    }
}
