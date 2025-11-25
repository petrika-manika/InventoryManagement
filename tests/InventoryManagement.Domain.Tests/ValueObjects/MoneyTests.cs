using FluentAssertions;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Create_WithValidAmount_ShouldCreateMoney()
    {
        // Arrange
        var amount = 100m;
        var currency = "ALL";

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        money.Should().NotBeNull();
        money.Amount.Should().Be(amount);
        money.Currency.Should().Be(currency);
    }

    [Fact]
    public void Create_WithNegativeAmount_ShouldThrowException()
    {
        // Arrange
        var amount = -50m;

        // Act
        Action act = () => Money.Create(amount);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*cannot be negative*");
    }

    [Fact]
    public void Create_WithNullCurrency_ShouldThrowException()
    {
        // Arrange
        var amount = 100m;

        // Act
        Action act = () => Money.Create(amount, null!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Currency*");
    }

    [Fact]
    public void Create_WithEmptyCurrency_ShouldThrowException()
    {
        // Arrange
        var amount = 100m;

        // Act
        Action act = () => Money.Create(amount, "");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Currency*");
    }

    [Fact]
    public void Create_WithDefaultCurrency_ShouldUseALL()
    {
        // Arrange
        var amount = 100m;

        // Act
        var money = Money.Create(amount);

        // Assert
        money.Currency.Should().Be("ALL");
    }

    [Fact]
    public void Create_WithLowercaseCurrency_ShouldConvertToUppercase()
    {
        // Arrange
        var amount = 100m;
        var currency = "usd";

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        money.Currency.Should().Be("USD");
    }

    [Fact]
    public void Add_SameCurrency_ShouldAddAmounts()
    {
        // Arrange
        var money1 = Money.Create(100m, "ALL");
        var money2 = Money.Create(50m, "ALL");

        // Act
        var result = money1.Add(money2);

        // Assert
        result.Amount.Should().Be(150m);
        result.Currency.Should().Be("ALL");
    }

    [Fact]
    public void Add_DifferentCurrency_ShouldThrowException()
    {
        // Arrange
        var money1 = Money.Create(100m, "ALL");
        var money2 = Money.Create(50m, "USD");

        // Act
        Action act = () => money1.Add(money2);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*different currencies*");
    }

    [Fact]
    public void Add_WithNull_ShouldThrowException()
    {
        // Arrange
        var money = Money.Create(100m, "ALL");

        // Act
        Action act = () => money.Add(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Subtract_ValidAmount_ShouldSubtractAmounts()
    {
        // Arrange
        var money1 = Money.Create(100m, "ALL");
        var money2 = Money.Create(30m, "ALL");

        // Act
        var result = money1.Subtract(money2);

        // Assert
        result.Amount.Should().Be(70m);
        result.Currency.Should().Be("ALL");
    }

    [Fact]
    public void Subtract_DifferentCurrency_ShouldThrowException()
    {
        // Arrange
        var money1 = Money.Create(100m, "ALL");
        var money2 = Money.Create(30m, "USD");

        // Act
        Action act = () => money1.Subtract(money2);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*different currencies*");
    }

    [Fact]
    public void Subtract_ResultingInNegative_ShouldThrowException()
    {
        // Arrange
        var money1 = Money.Create(50m, "ALL");
        var money2 = Money.Create(100m, "ALL");

        // Act
        Action act = () => money1.Subtract(money2);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*negative amount*");
    }

    [Fact]
    public void Subtract_WithNull_ShouldThrowException()
    {
        // Arrange
        var money = Money.Create(100m, "ALL");

        // Act
        Action act = () => money.Subtract(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Equals_SameAmountAndCurrency_ShouldReturnTrue()
    {
        // Arrange
        var money1 = Money.Create(100m, "ALL");
        var money2 = Money.Create(100m, "ALL");

        // Act & Assert
        money1.Equals(money2).Should().BeTrue();
        (money1 == money2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentAmount_ShouldReturnFalse()
    {
        // Arrange
        var money1 = Money.Create(100m, "ALL");
        var money2 = Money.Create(200m, "ALL");

        // Act & Assert
        money1.Equals(money2).Should().BeFalse();
        (money1 != money2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentCurrency_ShouldReturnFalse()
    {
        // Arrange
        var money1 = Money.Create(100m, "ALL");
        var money2 = Money.Create(100m, "USD");

        // Act & Assert
        money1.Equals(money2).Should().BeFalse();
        (money1 != money2).Should().BeTrue();
    }

    [Fact]
    public void Equals_CaseInsensitiveCurrency_ShouldReturnTrue()
    {
        // Arrange
        var money1 = Money.Create(100m, "ALL");
        var money2 = Money.Create(100m, "all");

        // Act & Assert
        money1.Equals(money2).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var money = Money.Create(100.50m, "ALL");

        // Act
        var result = money.ToString();

        // Assert
        result.Should().Be("100.50 ALL");
    }

    [Fact]
    public void GetHashCode_WithSameValue_ShouldReturnSameHashCode()
    {
        // Arrange
        var money1 = Money.Create(100m, "ALL");
        var money2 = Money.Create(100m, "all");

        // Act
        var hashCode1 = money1.GetHashCode();
        var hashCode2 = money2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }
}
