namespace InventoryManagement.Domain.ValueObjects;

/// <summary>
/// Represents a product name.
/// This is an immutable value object that ensures valid product names.
/// </summary>
public sealed class ProductName : IEquatable<ProductName>
{
    /// <summary>
    /// The minimum length for a product name.
    /// </summary>
    public const int MinLength = 2;

    /// <summary>
    /// The maximum length for a product name.
    /// </summary>
    public const int MaxLength = 200;

    /// <summary>
    /// Gets the product name value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductName"/> class.
    /// </summary>
    /// <param name="value">The product name.</param>
    private ProductName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new ProductName instance with validation.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <returns>A new ProductName instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the name is invalid.</exception>
    public static ProductName Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name cannot be null, empty, or whitespace.", nameof(name));
        }

        var trimmedName = name.Trim();

        if (trimmedName.Length < MinLength)
        {
            throw new ArgumentException($"Product name must be at least {MinLength} characters long.", nameof(name));
        }

        if (trimmedName.Length > MaxLength)
        {
            throw new ArgumentException($"Product name cannot exceed {MaxLength} characters.", nameof(name));
        }

        return new ProductName(trimmedName);
    }

    /// <summary>
    /// Returns the string representation of the product name.
    /// </summary>
    /// <returns>The product name value.</returns>
    public override string ToString()
    {
        return Value;
    }

    /// <summary>
    /// Implicitly converts a ProductName to a string.
    /// </summary>
    /// <param name="productName">The ProductName to convert.</param>
    public static implicit operator string(ProductName productName)
    {
        return productName?.Value ?? string.Empty;
    }

    /// <summary>
    /// Determines whether this ProductName instance is equal to another.
    /// Comparison is case-insensitive.
    /// </summary>
    public bool Equals(ProductName? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether this ProductName instance is equal to another object.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is ProductName other && Equals(other);
    }

    /// <summary>
    /// Returns the hash code for this ProductName instance.
    /// </summary>
    public override int GetHashCode()
    {
        return Value.ToUpperInvariant().GetHashCode();
    }

    /// <summary>
    /// Determines whether two ProductName instances are equal.
    /// </summary>
    public static bool operator ==(ProductName? left, ProductName? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two ProductName instances are not equal.
    /// </summary>
    public static bool operator !=(ProductName? left, ProductName? right)
    {
        return !(left == right);
    }
}
