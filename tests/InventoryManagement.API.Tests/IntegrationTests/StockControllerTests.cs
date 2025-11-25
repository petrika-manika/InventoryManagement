using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Application.Features.Products.Commands.AddStock;
using InventoryManagement.Application.Features.Products.Commands.CreateProduct;
using InventoryManagement.Application.Features.Products.Commands.RemoveStock;
using InventoryManagement.Application.Features.Users.Commands.LoginUser;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InventoryManagement.API.Tests.IntegrationTests;

/// <summary>
/// Integration tests for StockController.
/// Tests stock management operations including add, remove, and history tracking.
/// </summary>
public class StockControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public StockControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Helper method to get JWT token by logging in as admin.
    /// </summary>
    private async Task<string> GetJwtTokenAsync()
    {
        var loginCommand = new LoginUserCommand(
            Email: "admin@inventoryapp.com",
            Password: "Admin@123");

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginCommand);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        return result!.Token;
    }

    /// <summary>
    /// Helper method to create authenticated HTTP client with JWT token.
    /// </summary>
    private async Task<HttpClient> GetAuthenticatedClientAsync()
    {
        var client = _factory.CreateClient();
        var token = await GetJwtTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
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

    [Fact]
    public async Task GetHistory_ShouldReturnStockHistoryRecords()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var productId = await CreateTestProductAsync(authenticatedClient);

        // Create some stock history
        await authenticatedClient.PostAsJsonAsync("/api/stock/add", new AddStockCommand
        {
            ProductId = productId,
            Quantity = 50,
            Reason = "Initial stock"
        });

        await authenticatedClient.PostAsJsonAsync("/api/stock/add", new AddStockCommand
        {
            ProductId = productId,
            Quantity = 25,
            Reason = "Restock"
        });

        // Act
        var response = await authenticatedClient.GetAsync("/api/stock/history");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var history = await response.Content.ReadFromJsonAsync<List<StockHistoryDto>>();
        history.Should().NotBeNull();
        history.Should().NotBeEmpty();
        history.Should().Contain(h => h.ProductId == productId);
    }

    [Fact]
    public async Task GetHistory_WithProductId_ShouldReturnFilteredHistory()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var productId = await CreateTestProductAsync(authenticatedClient);

        // Create stock history for this product
        await authenticatedClient.PostAsJsonAsync("/api/stock/add", new AddStockCommand
        {
            ProductId = productId,
            Quantity = 50,
            Reason = "Test stock"
        });

        await authenticatedClient.PostAsJsonAsync("/api/stock/remove", new RemoveStockCommand
        {
            ProductId = productId,
            Quantity = 10,
            Reason = "Test removal"
        });

        // Act
        var response = await authenticatedClient.GetAsync($"/api/stock/history/{productId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var history = await response.Content.ReadFromJsonAsync<List<StockHistoryDto>>();
        history.Should().NotBeNull();
        history.Should().NotBeEmpty();
        history.Should().OnlyContain(h => h.ProductId == productId);
        history.Should().HaveCountGreaterOrEqualTo(2); // At least 2 operations
        
        // Verify we have both add and remove operations
        history.Should().Contain(h => h.ChangeType == "Added");
        history.Should().Contain(h => h.ChangeType == "Removed");
    }

    [Fact]
    public async Task GetHistory_WithDateFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var productId = await CreateTestProductAsync(authenticatedClient);

        // Create stock history
        await authenticatedClient.PostAsJsonAsync("/api/stock/add", new AddStockCommand
        {
            ProductId = productId,
            Quantity = 100,
            Reason = "Recent stock"
        });

        var fromDate = DateTime.UtcNow.AddMinutes(-5);
        var toDate = DateTime.UtcNow.AddMinutes(5);

        // Act
        var response = await authenticatedClient.GetAsync(
            $"/api/stock/history?fromDate={fromDate:O}&toDate={toDate:O}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var history = await response.Content.ReadFromJsonAsync<List<StockHistoryDto>>();
        history.Should().NotBeNull();
        history.Should().NotBeEmpty();
        history.Should().OnlyContain(h => h.ChangedAt >= fromDate && h.ChangedAt <= toDate);
    }

    [Fact]
    public async Task AddStock_ShouldCreateHistoryRecord()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var productId = await CreateTestProductAsync(authenticatedClient);

        var command = new AddStockCommand
        {
            ProductId = productId,
            Quantity = 75,
            Reason = "Test shipment"
        };

        // Act
        await authenticatedClient.PostAsJsonAsync("/api/stock/add", command);

        // Get history
        var historyResponse = await authenticatedClient.GetAsync($"/api/stock/history/{productId}");
        var history = await historyResponse.Content.ReadFromJsonAsync<List<StockHistoryDto>>();

        // Assert
        history.Should().NotBeNull();
        history.Should().Contain(h =>
            h.ProductId == productId &&
            h.QuantityChanged == 75 &&
            h.ChangeType == "Added" &&
            h.Reason == "Test shipment");
    }

    [Fact]
    public async Task RemoveStock_ShouldCreateHistoryRecord()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var productId = await CreateTestProductAsync(authenticatedClient);

        // Add stock first
        await authenticatedClient.PostAsJsonAsync("/api/stock/add", new AddStockCommand
        {
            ProductId = productId,
            Quantity = 100
        });

        // Remove stock
        var command = new RemoveStockCommand
        {
            ProductId = productId,
            Quantity = 25,
            Reason = "Test sale"
        };

        // Act
        await authenticatedClient.PostAsJsonAsync("/api/stock/remove", command);

        // Get history
        var historyResponse = await authenticatedClient.GetAsync($"/api/stock/history/{productId}");
        var history = await historyResponse.Content.ReadFromJsonAsync<List<StockHistoryDto>>();

        // Assert
        history.Should().NotBeNull();
        history.Should().Contain(h =>
            h.ProductId == productId &&
            h.QuantityChanged == -25 && // Negative for removal
            h.ChangeType == "Removed" &&
            h.Reason == "Test sale");
    }
}
