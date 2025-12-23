using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Application.Features.Products.Commands.AddStock;
using InventoryManagement.Application.Features.Products.Commands.CreateProduct;
using InventoryManagement.Application.Features.Products.Commands.RemoveStock;

namespace InventoryManagement.API.Tests.IntegrationTests;

/// <summary>
/// Integration tests for StockController.
/// Tests stock management operations including add, remove, and history tracking.
/// </summary>
public class StockControllerTests : IntegrationTestBase
{
    public StockControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    private async Task<Guid> CreateTestProductAsync(HttpClient authenticatedClient)
    {
        var command = new CreateAromaBombelCommand
        {
            Name = $"Stock Test Product {Guid.NewGuid()}",
            Description = "Test product for stock operations",
            Price = 1000m,
            Currency = "ALL"
        };

        var response = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", command);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    [Fact]
    public async Task AddStock_WithValidQuantity_ShouldIncreaseStock()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var productId = await CreateTestProductAsync(authenticatedClient);

        var command = new AddStockCommand
        {
            ProductId = productId,
            Quantity = 50,
            Reason = "New shipment"
        };

        // Act
        var response = await authenticatedClient.PostAsJsonAsync("/api/stock/add", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<StockOperationResult>();
        result.Should().NotBeNull();
        result!.ProductId.Should().Be(productId);
        result.NewStockQuantity.Should().Be(50);

        // Verify stock was actually updated
        var productResponse = await authenticatedClient.GetAsync($"/api/products/{productId}");
        var product = await productResponse.Content.ReadFromJsonAsync<ProductDto>();
        product!.StockQuantity.Should().Be(50);
    }

    [Fact]
    public async Task AddStock_WithInvalidProductId_ShouldReturnNotFound()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var invalidProductId = Guid.NewGuid();

        var command = new AddStockCommand
        {
            ProductId = invalidProductId,
            Quantity = 50,
            Reason = "Test"
        };

        // Act
        var response = await authenticatedClient.PostAsJsonAsync("/api/stock/add", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveStock_WithValidQuantity_ShouldDecreaseStock()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var productId = await CreateTestProductAsync(authenticatedClient);

        // Add stock first
        var addCommand = new AddStockCommand
        {
            ProductId = productId,
            Quantity = 100,
            Reason = "Initial stock"
        };
        await authenticatedClient.PostAsJsonAsync("/api/stock/add", addCommand);

        // Remove some stock
        var removeCommand = new RemoveStockCommand
        {
            ProductId = productId,
            Quantity = 30,
            Reason = "Sold"
        };

        // Act
        var response = await authenticatedClient.PostAsJsonAsync("/api/stock/remove", removeCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<StockOperationResult>();
        result.Should().NotBeNull();
        result!.ProductId.Should().Be(productId);
        result.NewStockQuantity.Should().Be(70);

        // Verify stock was actually updated
        var productResponse = await authenticatedClient.GetAsync($"/api/products/{productId}");
        var product = await productResponse.Content.ReadFromJsonAsync<ProductDto>();
        product!.StockQuantity.Should().Be(70);
    }

    [Fact]
    public async Task RemoveStock_WithInsufficientStock_ShouldReturnConflict()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var productId = await CreateTestProductAsync(authenticatedClient);

        // Add only 10 items
        var addCommand = new AddStockCommand
        {
            ProductId = productId,
            Quantity = 10,
            Reason = "Small stock"
        };
        await authenticatedClient.PostAsJsonAsync("/api/stock/add", addCommand);

        // Try to remove more than available
        var removeCommand = new RemoveStockCommand
        {
            ProductId = productId,
            Quantity = 50, // More than available
            Reason = "Test"
        };

        // Act
        var response = await authenticatedClient.PostAsJsonAsync("/api/stock/remove", removeCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Insufficient stock");
    }
}
