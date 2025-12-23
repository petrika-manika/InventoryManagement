using System.Net.Http.Headers;
using System.Net.Http.Json;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Application.Features.Users.Commands.LoginUser;

namespace InventoryManagement.API.Tests;

/// <summary>
/// Base class for integration tests.
/// Provides common setup and helper methods for all integration tests.
/// </summary>
public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly HttpClient Client;

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    /// <summary>
    /// Initializes the test by resetting the database.
    /// Called before each test method.
    /// </summary>
    public async Task InitializeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    /// <summary>
    /// Cleanup after test (if needed).
    /// Called after each test method.
    /// </summary>
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Helper method to get JWT token by logging in as admin.
    /// </summary>
    protected async Task<string> GetJwtTokenAsync(
        string email = "admin@inventoryapp.com", 
        string password = "Admin@123")
    {
        var loginCommand = new LoginUserCommand(email, password);

        var response = await Client.PostAsJsonAsync("/api/auth/login", loginCommand);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        return result!.Token;
    }

    /// <summary>
    /// Helper method to create authenticated HTTP client with JWT token.
    /// </summary>
    protected async Task<HttpClient> GetAuthenticatedClientAsync(
        string email = "admin@inventoryapp.com",
        string password = "Admin@123")
    {
        var client = Factory.CreateClient();
        var token = await GetJwtTokenAsync(email, password);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
