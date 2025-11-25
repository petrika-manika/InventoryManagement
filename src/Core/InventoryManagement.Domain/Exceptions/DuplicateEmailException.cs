namespace InventoryManagement.Domain.Exceptions;

public sealed class DuplicateEmailException : DomainException
{
    public string Email { get; }

    public DuplicateEmailException(string email)
        : base($"A user with email '{email}' already exists.")
    {
        Email = email;
    }
}
