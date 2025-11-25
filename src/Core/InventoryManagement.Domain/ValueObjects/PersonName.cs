namespace InventoryManagement.Domain.ValueObjects;

/// <summary>
/// Value object representing a person's name.
/// Encapsulates first and last name with validation.
/// </summary>
public sealed class PersonName : IEquatable<PersonName>
{
    /// <summary>
    /// Gets the first name.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Gets the last name.
    /// </summary>
    public string LastName { get; }

    /// <summary>
    /// Gets the full name combining first and last name.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonName"/> class.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <exception cref="ArgumentException">Thrown when firstName or lastName is invalid.</exception>
    public PersonName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null, empty, or whitespace.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null, empty, or whitespace.", nameof(lastName));
        }

        if (firstName.Length < 1 || firstName.Length > 50)
        {
            throw new ArgumentException("First name must be between 1 and 50 characters.", nameof(firstName));
        }

        if (lastName.Length < 1 || lastName.Length > 50)
        {
            throw new ArgumentException("Last name must be between 1 and 50 characters.", nameof(lastName));
        }

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    /// <summary>
    /// Determines whether the specified PersonName is equal to the current PersonName.
    /// </summary>
    /// <param name="other">The PersonName to compare with the current PersonName.</param>
    /// <returns>True if the specified PersonName is equal to the current PersonName; otherwise, false.</returns>
    public bool Equals(PersonName? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return FirstName == other.FirstName && LastName == other.LastName;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current PersonName.
    /// </summary>
    /// <param name="obj">The object to compare with the current PersonName.</param>
    /// <returns>True if the specified object is equal to the current PersonName; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return obj is PersonName other && Equals(other);
    }

    /// <summary>
    /// Returns the hash code for this PersonName.
    /// </summary>
    /// <returns>A hash code for the current PersonName.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName);
    }

    /// <summary>
    /// Returns a string representation of the PersonName.
    /// </summary>
    /// <returns>The full name.</returns>
    public override string ToString()
    {
        return FullName;
    }

    /// <summary>
    /// Determines whether two PersonName instances are equal.
    /// </summary>
    public static bool operator ==(PersonName? left, PersonName? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two PersonName instances are not equal.
    /// </summary>
    public static bool operator !=(PersonName? left, PersonName? right)
    {
        return !Equals(left, right);
    }
}
