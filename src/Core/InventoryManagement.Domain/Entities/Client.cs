using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Domain.Entities;

/// <summary>
/// Abstract base entity for all client types in the system.
/// Provides common properties and behaviors for Individual and Business clients.
/// </summary>
public abstract class Client
{
    private static readonly Regex PhoneRegex = new(@"^[\d\s\+\-\(\)]+$", RegexOptions.Compiled);

    /// <summary>
    /// Gets the unique identifier for the client.
    /// </summary>
    public string Id { get; protected set; } = null!;

    /// <summary>
    /// Gets the type of client (Individual or Business).
    /// </summary>
    public ClientType ClientType { get; protected set; }

    /// <summary>
    /// Gets the client's address.
    /// </summary>
    public string? Address { get; protected set; }

    /// <summary>
    /// Gets the client's email address.
    /// </summary>
    public string? Email { get; protected set; }

    /// <summary>
    /// Gets the client's phone number.
    /// </summary>
    public string? PhoneNumber { get; protected set; }

    /// <summary>
    /// Gets additional notes about the client.
    /// </summary>
    public string? Notes { get; protected set; }

    /// <summary>
    /// Gets the UTC timestamp when the client was created.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Gets the UTC timestamp when the client was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; protected set; }

    /// <summary>
    /// Gets the ID of the user who created this client.
    /// </summary>
    public string CreatedBy { get; protected set; } = null!;

    /// <summary>
    /// Gets the ID of the user who last updated this client.
    /// </summary>
    public string? UpdatedBy { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether the client is active.
    /// </summary>
    public bool IsActive { get; protected set; }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    protected Client()
    {
        // Required by EF Core for materialization
    }

    /// <summary>
    /// Validates the email address format.
    /// </summary>
    /// <exception cref="InvalidClientDataException">Thrown when the email format is invalid.</exception>
    protected void ValidateEmail()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            return; // Email is optional
        }

        var emailAttribute = new EmailAddressAttribute();
        if (!emailAttribute.IsValid(Email))
        {
            throw new InvalidClientDataException($"Invalid email format: '{Email}'.");
        }
    }

    /// <summary>
    /// Validates the phone number format.
    /// </summary>
    /// <exception cref="InvalidClientDataException">Thrown when the phone number format is invalid.</exception>
    protected void ValidatePhoneNumber()
    {
        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            return; // Phone number is optional
        }

        if (PhoneNumber.Length > 20)
        {
            throw new InvalidClientDataException($"Phone number cannot exceed 20 characters.");
        }

        if (!PhoneRegex.IsMatch(PhoneNumber))
        {
            throw new InvalidClientDataException(
                $"Invalid phone number format: '{PhoneNumber}'. Only digits, spaces, +, -, (, ) are allowed.");
        }
    }

    /// <summary>
    /// Updates the common fields shared by all client types.
    /// </summary>
    /// <param name="address">The new address.</param>
    /// <param name="email">The new email address.</param>
    /// <param name="phoneNumber">The new phone number.</param>
    /// <param name="notes">The new notes.</param>
    /// <exception cref="InvalidClientDataException">Thrown when email or phone number format is invalid.</exception>
    public void UpdateCommonFields(string? address, string? email, string? phoneNumber, string? notes)
    {
        // Validate before updating
        var tempEmail = Email;
        var tempPhone = PhoneNumber;

        Email = email;
        PhoneNumber = phoneNumber;

        try
        {
            ValidateEmail();
            ValidatePhoneNumber();
        }
        catch
        {
            // Restore original values if validation fails
            Email = tempEmail;
            PhoneNumber = tempPhone;
            throw;
        }

        Address = address;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the client (soft delete).
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activates the client.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
