using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Domain.Entities;

/// <summary>
/// Represents an individual client (person) in the system.
/// </summary>
public sealed class IndividualClient : Client
{
    /// <summary>
    /// Gets the client's first name.
    /// </summary>
    public string FirstName { get; private set; } = null!;

    /// <summary>
    /// Gets the client's last name.
    /// </summary>
    public string LastName { get; private set; } = null!;

    /// <summary>
    /// Gets the client's full name.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    private IndividualClient()
    {
        // Required by EF Core for materialization
    }

    /// <summary>
    /// Creates a new individual client.
    /// </summary>
    /// <param name="firstName">The client's first name.</param>
    /// <param name="lastName">The client's last name.</param>
    /// <param name="address">The client's address.</param>
    /// <param name="email">The client's email address.</param>
    /// <param name="phoneNumber">The client's phone number.</param>
    /// <param name="notes">Additional notes about the client.</param>
    /// <param name="createdBy">The ID of the user creating this client.</param>
    /// <returns>A new IndividualClient instance.</returns>
    /// <exception cref="InvalidClientDataException">Thrown when validation fails.</exception>
    public static IndividualClient Create(
        string firstName,
        string lastName,
        string? address,
        string? email,
        string? phoneNumber,
        string? notes,
        string createdBy)
    {
        var client = new IndividualClient
        {
            Id = Guid.NewGuid().ToString(),
            ClientType = ClientType.Individual,
            FirstName = ValidateAndTrimName(firstName, nameof(firstName)),
            LastName = ValidateAndTrimName(lastName, nameof(lastName)),
            Address = address,
            Email = email,
            PhoneNumber = phoneNumber,
            Notes = notes,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Validate email and phone number
        client.ValidateEmail();
        client.ValidatePhoneNumber();

        return client;
    }

    /// <summary>
    /// Updates the personal information of the individual client.
    /// </summary>
    /// <param name="firstName">The new first name.</param>
    /// <param name="lastName">The new last name.</param>
    /// <param name="address">The new address.</param>
    /// <param name="email">The new email address.</param>
    /// <param name="phoneNumber">The new phone number.</param>
    /// <param name="notes">The new notes.</param>
    /// <exception cref="InvalidClientDataException">Thrown when validation fails.</exception>
    public void UpdatePersonalInfo(
        string firstName,
        string lastName,
        string? address,
        string? email,
        string? phoneNumber,
        string? notes)
    {
        // Validate names
        var validatedFirstName = ValidateAndTrimName(firstName, nameof(firstName));
        var validatedLastName = ValidateAndTrimName(lastName, nameof(lastName));

        // Update common fields (this will validate email and phone)
        UpdateCommonFields(address, email, phoneNumber, notes);

        // Update name fields
        FirstName = validatedFirstName;
        LastName = validatedLastName;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates and trims a name field.
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
}
