namespace InventoryManagement.Domain.Exceptions;

/// <summary>
/// Exception thrown when client data validation fails.
/// Used for invalid email, phone number, or missing required fields.
/// </summary>
public sealed class InvalidClientDataException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidClientDataException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public InvalidClientDataException(string message)
        : base(message)
    {
    }
}
