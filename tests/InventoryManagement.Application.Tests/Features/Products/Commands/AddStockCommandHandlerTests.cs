using FluentAssertions;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Features.Products.Commands.AddStock;
using InventoryManagement.Application.Tests.Helpers;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryManagement.Application.Tests.Features.Products.Commands;

/// <summary>
/// Unit tests for AddStockCommandHandler.
/// Tests stock addition logic, history tracking, and validation.
/// </summary>
public class AddStockCommandHandlerTests
{
    private Mock<IApplicationDbContext> _contextMock;
    private Mock<ICurrentUserService> _currentUserServiceMock;
    private Mock<DbSet<Product>> _productsDbSetMock;
    private Mock<DbSet<StockHistory>> _stockHistoriesDbSetMock;
    private AddStockCommandHandler _handler;
    private readonly Guid _currentUserId = Guid.NewGuid();

    private void SetupTest(List<Product> products)
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _productsDbSetMock = new Mock<DbSet<Product>>();
        _stockHistoriesDbSetMock = new Mock<DbSet<StockHistory>>();

        var testData = new TestAsyncEnumerable<Product>(products);
        var queryable = testData as IQueryable<Product>;

        _productsDbSetMock.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(queryable.Provider);
        _productsDbSetMock.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(queryable.Expression);
        _productsDbSetMock.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        _productsDbSetMock.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        _productsDbSetMock.As<IAsyncEnumerable<Product>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(testData.GetAsyncEnumerator());

        _contextMock.Setup(x => x.Products).Returns(_productsDbSetMock.Object);
        _contextMock.Setup(x => x.StockHistories).Returns(_stockHistoriesDbSetMock.Object);

        _currentUserServiceMock.Setup(x => x.IsAuthenticated).Returns(true);
        _currentUserServiceMock.Setup(x => x.UserId).Returns(_currentUserId);

        _handler = new AddStockCommandHandler(_contextMock.Object, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidQuantity_ShouldIncreaseStock()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new AddStockCommand
        {
            ProductId = productId,
            Quantity = 50,
            Reason = "New shipment"
        };

        var product = AromaBombelProduct.Create(
            ProductName.Create("Test Product"),
            "Test",
            Money.Create(1000m, "ALL"),
            null,
            null);

        // Use reflection to set the Id since it's private set
        var idProperty = typeof(Product).GetProperty("Id");
        idProperty!.SetValue(product, productId);

        product.AddStock(100); // Initial stock: 100

        var products = new List<Product> { product };
        SetupTest(products);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(150); // 100 + 50
        product.StockQuantity.Should().Be(150);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidProductId_ShouldThrowProductNotFoundException()
    {
        // Arrange
        var nonExistentProductId = Guid.NewGuid();
        var command = new AddStockCommand
        {
            ProductId = nonExistentProductId,
            Quantity = 50,
            Reason = "New shipment"
        };

        // Setup empty product list
        var products = new List<Product>();
        SetupTest(products);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ProductNotFoundException>()
            .WithMessage($"*{nonExistentProductId}*");

        _stockHistoriesDbSetMock.Verify(x => x.Add(It.IsAny<StockHistory>()), Times.Never);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateStockHistoryRecord()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new AddStockCommand
        {
            ProductId = productId,
            Quantity = 25,
            Reason = "Restocking"
        };

        var product = AromaBombelProduct.Create(
            ProductName.Create("Test Product"),
            "Test",
            Money.Create(1000m, "ALL"),
            null,
            null);

        var idProperty = typeof(Product).GetProperty("Id");
        idProperty!.SetValue(product, productId);

        product.AddStock(75); // Initial stock: 75

        var products = new List<Product> { product };
        SetupTest(products);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(100); // 75 + 25

        _stockHistoriesDbSetMock.Verify(x => x.Add(It.Is<StockHistory>(sh =>
            sh.ProductId == productId &&
            sh.QuantityChanged == 25 &&
            sh.QuantityAfter == 100 &&
            sh.ChangeType == "Added" &&
            sh.Reason == "Restocking" &&
            sh.ChangedBy == _currentUserId
        )), Times.Once);

        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
