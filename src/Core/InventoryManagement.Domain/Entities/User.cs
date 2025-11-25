using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

public sealed class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public string FullName => $"{FirstName} {LastName}";

    // Private parameterless constructor for EF Core
    private User()
    {
    }

    public static User Create(
        string firstName,
        string lastName,
        Email email,
        string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null, empty, or whitespace.", nameof(firstName));
        }

        if (firstName.Length > 100)
        {
            throw new ArgumentException("First name cannot exceed 100 characters.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null, empty, or whitespace.", nameof(lastName));
        }

        if (lastName.Length > 100)
        {
            throw new ArgumentException("Last name cannot exceed 100 characters.", nameof(lastName));
        }

        if (email is null)
        {
            throw new ArgumentNullException(nameof(email), "Email cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash cannot be null, empty, or whitespace.", nameof(passwordHash));
        }

        var now = DateTime.UtcNow;

        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email,
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public void UpdateInformation(string firstName, string lastName, Email email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null, empty, or whitespace.", nameof(firstName));
        }

        if (firstName.Length > 100)
        {
            throw new ArgumentException("First name cannot exceed 100 characters.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null, empty, or whitespace.", nameof(lastName));
        }

        if (lastName.Length > 100)
        {
            throw new ArgumentException("Last name cannot exceed 100 characters.", nameof(lastName));
        }

        if (email is null)
        {
            throw new ArgumentNullException(nameof(email), "Email cannot be null.");
        }

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
        {
            throw new ArgumentException("Password hash cannot be null, empty, or whitespace.", nameof(newPasswordHash));
        }

        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
