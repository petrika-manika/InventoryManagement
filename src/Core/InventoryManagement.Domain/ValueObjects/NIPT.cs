using System.Text.RegularExpressions;

namespace InventoryManagement.Domain.ValueObjects;

/// <summary>
/// Value object representing a NIPT (Albanian business tax ID).
/// Encapsulates NIPT validation rules.
/// </summary>
public sealed class NIPT : IEquatable<NIPT>
{
    private static readonly Regex NiptRegex = new(@"^[A-Z0-9]{10}$", RegexOptions.Compiled);

    /// <summary>
    /// Gets the NIPT value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NIPT"/> class.
    /// </summary>
    /// <param name="value">The NIPT value.</param>
    /// <exception cref="ArgumentException">Thrown when the NIPT value is invalid.</exception>
    public NIPT(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("NIPT cannot be null, empty, or whitespace.", nameof(value));
        }

        var trimmedValue = value.Trim().ToUpperInvariant();

        if (!NiptRegex.IsMatch(trimmedValue))
        {
            throw new ArgumentException(
                "NIPT must be exactly 10 alphanumeric characters (letters and digits).",
                nameof(value));
        }

        Value = trimmedValue;
    }

    /// <summary>
    /// Validates whether a string is a valid NIPT format without throwing an exception.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>True if the value is a valid NIPT format; otherwise, false.</returns>
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmedValue = value.Trim().ToUpperInvariant();
        return NiptRegex.IsMatch(trimmedValue);
    }

    /// <summary>
    /// Determines whether the specified NIPT is equal to the current NIPT.
    /// </summary>
    /// <param name="other">The NIPT to compare with the current NIPT.</param>
    /// <returns>True if the specified NIPT is equal to the current NIPT; otherwise, false.</returns>
    public bool Equals(NIPT? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current NIPT.
    /// </summary>
    /// <param name="obj">The object to compare with the current NIPT.</param>
    /// <returns>True if the specified object is equal to the current NIPT; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return obj is NIPT other && Equals(other);
    }

    /// <summary>
    /// Returns the hash code for this NIPT.
    /// </summary>
    /// <returns>A hash code for the current NIPT.</returns>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Returns a string representation of the NIPT.
    /// </summary>
    /// <returns>The NIPT value.</returns>
    public override string ToString()
    {
        return Value;
    }

    /// <summary>
    /// Determines whether two NIPT instances are equal.
    /// </summary>
    public static bool operator ==(NIPT? left, NIPT? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two NIPT instances are not equal.
    /// </summary>
    public static bool operator !=(NIPT? left, NIPT? right)
    {
        return !Equals(left, right);
    }
}
