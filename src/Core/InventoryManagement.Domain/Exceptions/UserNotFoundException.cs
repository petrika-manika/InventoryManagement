namespace InventoryManagement.Domain.Exceptions;

public sealed class UserNotFoundException : DomainException
{
    public Guid? UserId { get; }
    public string? Email { get; }

    public UserNotFoundException(Guid userId)
        : base($"User with ID '{userId}' was not found.")
    {
        UserId = userId;
    }

    public UserNotFoundException(string email)
        : base($"User with email '{email}' was not found.")
    {
        Email = email;
    }
}
