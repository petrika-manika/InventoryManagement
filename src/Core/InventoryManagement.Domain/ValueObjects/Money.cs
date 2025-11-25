namespace InventoryManagement.Domain.ValueObjects;

/// <summary>
/// Represents a monetary amount with currency.
/// This is an immutable value object that ensures valid monetary values.
/// </summary>
public sealed class Money : IEquatable<Money>
{
    /// <summary>
    /// Gets the monetary amount. Must be greater than or equal to zero.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Gets the currency code (e.g., "ALL" for Albanian Lek).
    /// </summary>
    public string Currency { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Money"/> class.
    /// </summary>
    /// <param name="amount">The monetary amount.</param>
    /// <param name="currency">The currency code.</param>
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Creates a new Money instance with validation.
    /// </summary>
    /// <param name="amount">The monetary amount.</param>
    /// <param name="currency">The currency code. Defaults to "ALL" (Albanian Lek).</param>
    /// <returns>A new Money instance.</returns>
    /// <exception cref="ArgumentException">Thrown when amount is negative or currency is invalid.</exception>
    public static Money Create(decimal amount, string currency = "ALL")
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot be null or empty.", nameof(currency));
        }

        return new Money(amount, currency.ToUpperInvariant());
    }

    /// <summary>
    /// Adds two Money objects together.
    /// </summary>
    /// <param name="other">The Money to add.</param>
    /// <returns>A new Money object representing the sum.</returns>
    /// <exception cref="InvalidOperationException">Thrown when currencies don't match.</exception>
    public Money Add(Money other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        if (!Currency.Equals(other.Currency, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Cannot add money with different currencies: {Currency} and {other.Currency}");
        }

        return new Money(Amount + other.Amount, Currency);
    }

    /// <summary>
    /// Subtracts one Money object from another.
    /// </summary>
    /// <param name="other">The Money to subtract.</param>
    /// <returns>A new Money object representing the difference.</returns>
    /// <exception cref="InvalidOperationException">Thrown when currencies don't match.</exception>
    public Money Subtract(Money other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        if (!Currency.Equals(other.Currency, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Cannot subtract money with different currencies: {Currency} and {other.Currency}");
        }

        var result = Amount - other.Amount;
        if (result < 0)
        {
            throw new InvalidOperationException("Subtraction would result in a negative amount.");
        }

        return new Money(result, Currency);
    }

    /// <summary>
    /// Returns a string representation of the money value.
    /// </summary>
    /// <returns>A formatted string like "100.00 ALL".</returns>
    public override string ToString()
    {
        return $"{Amount:F2} {Currency}";
    }

    /// <summary>
    /// Determines whether this Money instance is equal to another.
    /// </summary>
    public bool Equals(Money? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Amount == other.Amount && 
               Currency.Equals(other.Currency, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether this Money instance is equal to another object.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Money other && Equals(other);
    }

    /// <summary>
    /// Returns the hash code for this Money instance.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency.ToUpperInvariant());
    }

    /// <summary>
    /// Determines whether two Money instances are equal.
    /// </summary>
    public static bool operator ==(Money? left, Money? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two Money instances are not equal.
    /// </summary>
    public static bool operator !=(Money? left, Money? right)
    {
        return !(left == right);
    }
}
