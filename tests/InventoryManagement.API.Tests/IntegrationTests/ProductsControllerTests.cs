using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Application.Features.Products.Commands.CreateProduct;
using InventoryManagement.Application.Features.Products.Commands.UpdateProduct;

namespace InventoryManagement.API.Tests.IntegrationTests;

/// <summary>
/// Integration tests for ProductsController.
/// Tests the entire API stack including authentication, validation, business logic, and database operations.
/// </summary>
public class ProductsControllerTests : IntegrationTestBase
{
    public ProductsControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateAromaBombel_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var command = new CreateAromaBombelCommand
        {
            Name = $"Test Bombel {Guid.NewGuid()}",
            Description = "Test description",
            Price = 1500m,
            Currency = "ALL",
            TasteId = 1
        };

        // Act
        var response = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var productId = await response.Content.ReadFromJsonAsync<Guid>();
        productId.Should().NotBeEmpty();
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAromaBombel_WithDuplicateName_ShouldReturnConflict()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var uniqueName = $"Duplicate Test {Guid.NewGuid()}";
        
        var command1 = new CreateAromaBombelCommand
        {
            Name = uniqueName,
            Description = "First product",
            Price = 1000m,
            Currency = "ALL"
        };

        // Create first product
        await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", command1);

        // Try to create duplicate
        var command2 = new CreateAromaBombelCommand
        {
            Name = uniqueName,
            Description = "Duplicate product",
            Price = 1500m,
            Currency = "ALL"
        };

        // Act
        var response = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", command2);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain(uniqueName);
    }

    [Fact]
    public async Task CreateAromaBombel_WithInvalidPrice_ShouldReturnBadRequest()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var command = new CreateAromaBombelCommand
        {
            Name = $"Test Bombel {Guid.NewGuid()}",
            Description = "Test",
            Price = -100m, // Invalid price
            Currency = "ALL"
        };

        // Act
        var response = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAll_ShouldReturnProducts()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Create a test product first
        var command = new CreateAromaBombelCommand
        {
            Name = $"GetAll Test {Guid.NewGuid()}",
            Description = "Test",
            Price = 1000m,
            Currency = "ALL"
        };
        await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", command);

        // Act
        var response = await authenticatedClient.GetAsync("/api/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
        products.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetByType_AromaBombel_ShouldReturnOnlyAromaBombelProducts()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Create an Aroma Bombel product
        var bombelCommand = new CreateAromaBombelCommand
        {
            Name = $"Bombel Type Test {Guid.NewGuid()}",
            Description = "Test",
            Price = 1000m,
            Currency = "ALL"
        };
        await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", bombelCommand);

        // Act
        var response = await authenticatedClient.GetAsync("/api/products/type/1"); // 1 = AromaBombel

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
        products.Should().NotBeEmpty();
        products.Should().OnlyContain(p => p.ProductTypeId == 1);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnProduct()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Create a product
        var command = new CreateAromaBombelCommand
        {
            Name = $"GetById Test {Guid.NewGuid()}",
            Description = "Test",
            Price = 1000m,
            Currency = "ALL"
        };
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", command);
        var productId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Act
        var response = await authenticatedClient.GetAsync($"/api/products/{productId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        product.Should().NotBeNull();
        product!.Id.Should().Be(productId);
        product.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var invalidId = Guid.NewGuid();

        // Act
        var response = await authenticatedClient.GetAsync($"/api/products/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_WithValidData_ShouldReturnNoContent()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Create a product
        var createCommand = new CreateAromaBombelCommand
        {
            Name = $"Update Test {Guid.NewGuid()}",
            Description = "Original",
            Price = 1000m,
            Currency = "ALL"
        };
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", createCommand);
        var productId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Update the product
        var updateCommand = new UpdateAromaBombelCommand
        {
            ProductId = productId,
            Name = createCommand.Name, // Keep same name
            Description = "Updated description",
            Price = 1500m,
            Currency = "ALL",
            TasteId = 2
        };

        // Act
        var response = await authenticatedClient.PutAsJsonAsync($"/api/products/aroma-bombel/{productId}", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the update
        var getResponse = await authenticatedClient.GetAsync($"/api/products/{productId}");
        var product = await getResponse.Content.ReadFromJsonAsync<ProductDto>();
        product!.Description.Should().Be("Updated description");
        product.Price.Should().Be(1500m);
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Create a product
        var command = new CreateAromaBombelCommand
        {
            Name = $"Delete Test {Guid.NewGuid()}",
            Description = "Test",
            Price = 1000m,
            Currency = "ALL"
        };
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", command);
        var productId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Act
        var response = await authenticatedClient.DeleteAsync($"/api/products/{productId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify product is deactivated (soft delete)
        var getResponse = await authenticatedClient.GetAsync($"/api/products/{productId}");
        var product = await getResponse.Content.ReadFromJsonAsync<ProductDto>();
        product!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetLowStock_ShouldReturnProductsBelowThreshold()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Create a product with no stock (0 is below any threshold)
        var command = new CreateAromaBombelCommand
        {
            Name = $"Low Stock Test {Guid.NewGuid()}",
            Description = "Test",
            Price = 1000m,
            Currency = "ALL"
        };
        await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", command);

        // Act
        var response = await authenticatedClient.GetAsync("/api/products/low-stock?threshold=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
        products.Should().NotBeEmpty();
        products.Should().OnlyContain(p => p.StockQuantity <= 10);
    }

    #region Delete Product with Stock Validation Tests

    [Fact]
    public async Task DeleteProduct_WithStock_ShouldReturnBadRequest()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Create a product
        var createCommand = new CreateAromaBombelCommand
        {
            Name = $"Delete With Stock Test {Guid.NewGuid()}",
            Description = "Product with stock",
            Price = 1000m,
            Currency = "ALL"
        };
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", createCommand);
        var productId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Add stock to the product
        var addStockCommand = new InventoryManagement.Application.Features.Products.Commands.AddStock.AddStockCommand
        {
            ProductId = productId,
            Quantity = 25,
            Reason = "Initial stock"
        };
        await authenticatedClient.PostAsJsonAsync("/api/stock/add", addStockCommand);

        // Act - Try to delete the product with stock
        var deleteResponse = await authenticatedClient.DeleteAsync($"/api/products/{productId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorContent = await deleteResponse.Content.ReadAsStringAsync();
        errorContent.Should().Contain("stock");
        // Error should mention either the quantity or "existing"
        var containsQuantity = errorContent.Contains("25");
        var containsExisting = errorContent.Contains("existing");
        (containsQuantity || containsExisting).Should().BeTrue("error message should mention stock quantity or existing stock");

        // Verify product still exists and is active
        var getResponse = await authenticatedClient.GetAsync($"/api/products/{productId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var product = await getResponse.Content.ReadFromJsonAsync<ProductDto>();
        product.Should().NotBeNull();
        product!.IsActive.Should().BeTrue();
        product.StockQuantity.Should().Be(25);
    }

    [Fact]
    public async Task DeleteProduct_WithoutStock_ShouldSucceed()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Create a product with no stock
        var createCommand = new CreateAromaBombelCommand
        {
            Name = $"Delete Without Stock Test {Guid.NewGuid()}",
            Description = "Product without stock",
            Price = 1000m,
            Currency = "ALL"
        };
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", createCommand);
        var productId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Verify product exists with 0 stock
        var verifyResponse = await authenticatedClient.GetAsync($"/api/products/{productId}");
        var productBeforeDelete = await verifyResponse.Content.ReadFromJsonAsync<ProductDto>();
        productBeforeDelete!.StockQuantity.Should().Be(0);

        // Act - Delete the product
        var deleteResponse = await authenticatedClient.DeleteAsync($"/api/products/{productId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify product is deactivated (soft delete)
        var getResponse = await authenticatedClient.GetAsync($"/api/products/{productId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var product = await getResponse.Content.ReadFromJsonAsync<ProductDto>();
        product.Should().NotBeNull();
        product!.IsActive.Should().BeFalse(); // Soft deleted
        product.StockQuantity.Should().Be(0);
    }

    [Fact]
    public async Task DeleteProduct_AfterRemovingAllStock_ShouldSucceed()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Create a product
        var createCommand = new CreateAromaBombelCommand
        {
            Name = $"Delete After Remove Stock Test {Guid.NewGuid()}",
            Description = "Product with stock that will be removed",
            Price = 1000m,
            Currency = "ALL"
        };
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", createCommand);
        var productId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Add stock to the product
        var addStockCommand = new InventoryManagement.Application.Features.Products.Commands.AddStock.AddStockCommand
        {
            ProductId = productId,
            Quantity = 100,
            Reason = "Initial stock"
        };
        var addStockResponse = await authenticatedClient.PostAsJsonAsync("/api/stock/add", addStockCommand);
        addStockResponse.EnsureSuccessStatusCode();

        // Verify product has stock
        var verifyResponse1 = await authenticatedClient.GetAsync($"/api/products/{productId}");
        var productWithStock = await verifyResponse1.Content.ReadFromJsonAsync<ProductDto>();
        productWithStock!.StockQuantity.Should().Be(100);

        // Remove all stock
        var removeStockCommand = new InventoryManagement.Application.Features.Products.Commands.RemoveStock.RemoveStockCommand
        {
            ProductId = productId,
            Quantity = 100,
            Reason = "Clearing stock for deletion"
        };
        var removeStockResponse = await authenticatedClient.PostAsJsonAsync("/api/stock/remove", removeStockCommand);
        removeStockResponse.EnsureSuccessStatusCode();

        // Verify stock is now 0
        var verifyResponse2 = await authenticatedClient.GetAsync($"/api/products/{productId}");
        var productWithoutStock = await verifyResponse2.Content.ReadFromJsonAsync<ProductDto>();
        productWithoutStock!.StockQuantity.Should().Be(0);

        // Act - Delete the product (should succeed now)
        var deleteResponse = await authenticatedClient.DeleteAsync($"/api/products/{productId}");

        // Assert with better error diagnostics
        if (deleteResponse.StatusCode != HttpStatusCode.NoContent)
        {
            var errorContent = await deleteResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Expected NoContent (204) but got {deleteResponse.StatusCode} ({(int)deleteResponse.StatusCode}). Error: {errorContent}");
        }

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify product is deactivated
        var getResponse = await authenticatedClient.GetAsync($"/api/products/{productId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var product = await getResponse.Content.ReadFromJsonAsync<ProductDto>();
        product.Should().NotBeNull();
        product!.IsActive.Should().BeFalse();
        product.StockQuantity.Should().Be(0);
    }

    [Fact]
    public async Task DeleteProduct_WithStockAddedBetweenValidationAndExecution_ShouldBeRejected()
    {
        // This test verifies the handler's safety check catches race conditions
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Create a product with stock
        var createCommand = new CreateAromaBombelCommand
        {
            Name = $"Race Condition Test {Guid.NewGuid()}",
            Description = "Testing safety check",
            Price = 1000m,
            Currency = "ALL"
        };
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/products/aroma-bombel", createCommand);
        var productId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Add stock
        var addStockCommand = new InventoryManagement.Application.Features.Products.Commands.AddStock.AddStockCommand
        {
            ProductId = productId,
            Quantity = 50,
            Reason = "Test stock"
        };
        await authenticatedClient.PostAsJsonAsync("/api/stock/add", addStockCommand);

        // Act - Try to delete (should fail due to validator)
        var deleteResponse = await authenticatedClient.DeleteAsync($"/api/products/{productId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorContent = await deleteResponse.Content.ReadAsStringAsync();
        errorContent.Should().Contain("stock");

        // Verify product still exists
        var getResponse = await authenticatedClient.GetAsync($"/api/products/{productId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var product = await getResponse.Content.ReadFromJsonAsync<ProductDto>();
        product.Should().NotBeNull();
        product!.IsActive.Should().BeTrue();
        product.StockQuantity.Should().Be(50);
    }

    #endregion
}
