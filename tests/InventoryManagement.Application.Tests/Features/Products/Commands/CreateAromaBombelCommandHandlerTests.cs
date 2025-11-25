using FluentAssertions;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Features.Products.Commands.CreateProduct;
using InventoryManagement.Application.Tests.Helpers;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryManagement.Application.Tests.Features.Products.Commands;

/// <summary>
/// Unit tests for CreateAromaBombelCommandHandler.
/// Tests product creation logic, validation, and duplicate name detection.
/// </summary>
public class CreateAromaBombelCommandHandlerTests
{
    private Mock<IApplicationDbContext> _contextMock;
    private Mock<DbSet<Product>> _productsDbSetMock;
    private CreateAromaBombelCommandHandler _handler;

    private void SetupTest(List<Product> products)
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _productsDbSetMock = new Mock<DbSet<Product>>();

        var testData = new TestAsyncEnumerable<Product>(products);
        var queryable = testData as IQueryable<Product>;

        _productsDbSetMock.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(queryable.Provider);
        _productsDbSetMock.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(queryable.Expression);
        _productsDbSetMock.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        _productsDbSetMock.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        _productsDbSetMock.As<IAsyncEnumerable<Product>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(testData.GetAsyncEnumerator());

        _contextMock.Setup(x => x.Products).Returns(_productsDbSetMock.Object);

        _handler = new CreateAromaBombelCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateProduct()
    {
        // Arrange
        var command = new CreateAromaBombelCommand
        {
            Name = "Lavender Bombel",
            Description = "Relaxing lavender aroma",
            Price = 1500m,
            Currency = "ALL",
            PhotoUrl = "https://example.com/lavender.jpg",
            TasteId = (int)TasteType.Flower
        };

        // Setup empty product list (no duplicates)
        var products = new List<Product>();
        SetupTest(products);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _productsDbSetMock.Verify(x => x.Add(It.Is<Product>(p =>
            p.Name.Value == command.Name &&
            p.Price.Amount == command.Price &&
            p.ProductType == ProductType.AromaBombel
        )), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateName_ShouldThrowDuplicateProductNameException()
    {
        // Arrange
        var command = new CreateAromaBombelCommand
        {
            Name = "Existing Bombel",
            Description = "Test",
            Price = 1000m,
            Currency = "ALL"
        };

        // Setup existing product with same name
        var existingProduct = AromaBombelProduct.Create(
            Domain.ValueObjects.ProductName.Create("Existing Bombel"),
            "Existing",
            Domain.ValueObjects.Money.Create(1000m, "ALL"),
            null,
            TasteType.Flower);

        var products = new List<Product> { existingProduct };
        SetupTest(products);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DuplicateProductNameException>()
            .WithMessage("*Existing Bombel*")
            .WithMessage("*AromaBombel*");

        _productsDbSetMock.Verify(x => x.Add(It.IsAny<Product>()), Times.Never);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidPrice_ShouldThrowException()
    {
        // Arrange
        var products = new List<Product>();
        SetupTest(products);

        var command = new CreateAromaBombelCommand
        {
            Name = "Test Bombel",
            Description = "Test",
            Price = -100m, // Negative price
            Currency = "ALL"
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*cannot be negative*");
    }

    [Fact]
    public async Task Handle_WithValidTaste_ShouldSetTasteProperty()
    {
        // Arrange
        var command = new CreateAromaBombelCommand
        {
            Name = "Sweet Bombel",
            Description = "Sweet aroma",
            Price = 1200m,
            Currency = "ALL",
            TasteId = (int)TasteType.Sweet
        };

        // Setup empty product list
        var products = new List<Product>();
        SetupTest(products);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _productsDbSetMock.Verify(x => x.Add(It.Is<AromaBombelProduct>(p =>
            p.Taste == TasteType.Sweet
        )), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNullTaste_ShouldCreateProductWithoutTaste()
    {
        // Arrange
        var command = new CreateAromaBombelCommand
        {
            Name = "Plain Bombel",
            Description = "No specific taste",
            Price = 1000m,
            Currency = "ALL",
            TasteId = null
        };

        // Setup empty product list
        var products = new List<Product>();
        SetupTest(products);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _productsDbSetMock.Verify(x => x.Add(It.Is<AromaBombelProduct>(p =>
            p.Taste == null
        )), Times.Once);
    }
}
