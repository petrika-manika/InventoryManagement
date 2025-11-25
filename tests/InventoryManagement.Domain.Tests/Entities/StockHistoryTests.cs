using FluentAssertions;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Tests.Entities;

public class StockHistoryTests
{
    [Fact]
    public void CreateAddition_ShouldCreateWithPositiveQuantity()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantityAdded = 50;
        var quantityAfter = 150;
        var reason = "Initial stock";
        var changedBy = Guid.NewGuid();

        // Act
        var stockHistory = StockHistory.CreateAddition(
            productId,
            quantityAdded,
            quantityAfter,
            reason,
            changedBy);

        // Assert
        stockHistory.Should().NotBeNull();
        stockHistory.Id.Should().NotBeEmpty();
        stockHistory.ProductId.Should().Be(productId);
        stockHistory.QuantityChanged.Should().Be(quantityAdded);
        stockHistory.QuantityAfter.Should().Be(quantityAfter);
        stockHistory.Reason.Should().Be(reason);
        stockHistory.ChangedBy.Should().Be(changedBy);
        stockHistory.ChangedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CreateAddition_ShouldSetChangeTypeToAdded()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var changedBy = Guid.NewGuid();

        // Act
        var stockHistory = StockHistory.CreateAddition(
            productId,
            10,
            10,
            null,
            changedBy);

        // Assert
        stockHistory.ChangeType.Should().Be("Added");
    }

    [Fact]
    public void CreateAddition_WithZeroQuantity_ShouldThrowException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var changedBy = Guid.NewGuid();

        // Act
        Action act = () => StockHistory.CreateAddition(
            productId,
            0,
            100,
            null,
            changedBy);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*must be positive*");
    }

    [Fact]
    public void CreateAddition_WithNegativeQuantity_ShouldThrowException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var changedBy = Guid.NewGuid();

        // Act
        Action act = () => StockHistory.CreateAddition(
            productId,
            -10,
            100,
            null,
            changedBy);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*must be positive*");
    }

    [Fact]
    public void CreateAddition_WithEmptyProductId_ShouldThrowException()
    {
        // Arrange
        var changedBy = Guid.NewGuid();

        // Act
        Action act = () => StockHistory.CreateAddition(
            Guid.Empty,
            10,
            10,
            null,
            changedBy);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Product ID*");
    }

    [Fact]
    public void CreateAddition_WithEmptyChangedBy_ShouldThrowException()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        Action act = () => StockHistory.CreateAddition(
            productId,
            10,
            10,
            null,
            Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*user ID*");
    }

    [Fact]
    public void CreateRemoval_ShouldCreateWithNegativeQuantity()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantityRemoved = 30;
        var quantityAfter = 70;
        var reason = "Sold items";
        var changedBy = Guid.NewGuid();

        // Act
        var stockHistory = StockHistory.CreateRemoval(
            productId,
            quantityRemoved,
            quantityAfter,
            reason,
            changedBy);

        // Assert
        stockHistory.Should().NotBeNull();
        stockHistory.Id.Should().NotBeEmpty();
        stockHistory.ProductId.Should().Be(productId);
        stockHistory.QuantityChanged.Should().Be(-quantityRemoved);
        stockHistory.QuantityAfter.Should().Be(quantityAfter);
        stockHistory.Reason.Should().Be(reason);
        stockHistory.ChangedBy.Should().Be(changedBy);
        stockHistory.ChangedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CreateRemoval_ShouldSetChangeTypeToRemoved()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var changedBy = Guid.NewGuid();

        // Act
        var stockHistory = StockHistory.CreateRemoval(
            productId,
            10,
            90,
            null,
            changedBy);

        // Assert
        stockHistory.ChangeType.Should().Be("Removed");
    }

    [Fact]
    public void CreateRemoval_WithZeroQuantity_ShouldThrowException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var changedBy = Guid.NewGuid();

        // Act
        Action act = () => StockHistory.CreateRemoval(
            productId,
            0,
            100,
            null,
            changedBy);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*must be positive*");
    }

    [Fact]
    public void CreateRemoval_WithNegativeQuantity_ShouldThrowException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var changedBy = Guid.NewGuid();

        // Act
        Action act = () => StockHistory.CreateRemoval(
            productId,
            -10,
            100,
            null,
            changedBy);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*must be positive*");
    }

    [Fact]
    public void CreateRemoval_WithEmptyProductId_ShouldThrowException()
    {
        // Arrange
        var changedBy = Guid.NewGuid();

        // Act
        Action act = () => StockHistory.CreateRemoval(
            Guid.Empty,
            10,
            90,
            null,
            changedBy);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Product ID*");
    }

    [Fact]
    public void CreateRemoval_WithEmptyChangedBy_ShouldThrowException()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        Action act = () => StockHistory.CreateRemoval(
            productId,
            10,
            90,
            null,
            Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*user ID*");
    }

    [Fact]
    public void CreateAddition_WithNullReason_ShouldCreateSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var changedBy = Guid.NewGuid();

        // Act
        var stockHistory = StockHistory.CreateAddition(
            productId,
            10,
            10,
            null,
            changedBy);

        // Assert
        stockHistory.Should().NotBeNull();
        stockHistory.Reason.Should().BeNull();
    }

    [Fact]
    public void CreateRemoval_WithNullReason_ShouldCreateSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var changedBy = Guid.NewGuid();

        // Act
        var stockHistory = StockHistory.CreateRemoval(
            productId,
            10,
            90,
            null,
            changedBy);

        // Assert
        stockHistory.Should().NotBeNull();
        stockHistory.Reason.Should().BeNull();
    }

    [Fact]
    public void CreateAddition_QuantityChangedShouldBePositive()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var changedBy = Guid.NewGuid();
        var quantityAdded = 50;

        // Act
        var stockHistory = StockHistory.CreateAddition(
            productId,
            quantityAdded,
            50,
            "Test",
            changedBy);

        // Assert
        stockHistory.QuantityChanged.Should().BePositive();
        stockHistory.QuantityChanged.Should().Be(quantityAdded);
    }

    [Fact]
    public void CreateRemoval_QuantityChangedShouldBeNegative()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var changedBy = Guid.NewGuid();
        var quantityRemoved = 30;

        // Act
        var stockHistory = StockHistory.CreateRemoval(
            productId,
            quantityRemoved,
            70,
            "Test",
            changedBy);

        // Assert
        stockHistory.QuantityChanged.Should().BeNegative();
        stockHistory.QuantityChanged.Should().Be(-quantityRemoved);
    }
}
