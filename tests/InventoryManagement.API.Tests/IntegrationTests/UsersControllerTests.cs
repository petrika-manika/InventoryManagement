using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Application.Features.Users.Commands.CreateUser;
using InventoryManagement.Application.Features.Users.Commands.LoginUser;
using InventoryManagement.Application.Features.Users.Commands.UpdateUser;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace InventoryManagement.API.Tests.IntegrationTests;

/// <summary>
/// Integration tests for UsersController endpoints.
/// Tests complete CRUD operations with authentication.
/// </summary>
public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public UsersControllerTests(WebApplicationFactory<Program> factory)
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

    [Fact]
    public async Task GetAll_WithAuthentication_ShouldReturnUsers()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Act
        var response = await authenticatedClient.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        users.Should().NotBeNull();
        users.Should().NotBeEmpty();
        users.Should().Contain(u => u.Email == "admin@inventoryapp.com");
    }

    [Fact]
    public async Task GetAll_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var command = new CreateUserCommand(
            FirstName: "Test",
            LastName: "User",
            Email: $"test.user.{Guid.NewGuid()}@example.com", // Unique email
            Password: "Test@123");

        // Act
        var response = await authenticatedClient.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var userId = await response.Content.ReadFromJsonAsync<Guid>();
        userId.Should().NotBeEmpty();
        
        // Verify Location header (case-insensitive check)
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().ToLowerInvariant().Should().Contain($"/api/users/{userId}".ToLowerInvariant());
    }

    [Fact]
    public async Task Create_WithDuplicateEmail_ShouldReturnConflict()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        var command = new CreateUserCommand(
            FirstName: "Duplicate",
            LastName: "User",
            Email: "admin@inventoryapp.com", // Admin email already exists
            Password: "Test@123");

        // Act
        var response = await authenticatedClient.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnUser()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        
        // First create a user
        var createCommand = new CreateUserCommand(
            FirstName: "John",
            LastName: "Doe",
            Email: $"john.doe.{Guid.NewGuid()}@example.com",
            Password: "Test@123");
        
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/users", createCommand);
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Act
        var response = await authenticatedClient.GetAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        user.Should().NotBeNull();
        user!.Id.Should().Be(userId);
        user.FirstName.Should().Be("John");
        user.LastName.Should().Be("Doe");
        user.Email.Should().Be(createCommand.Email);
    }

    [Fact]
    public async Task Update_WithValidData_ShouldUpdateUser()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        
        // First create a user
        var createCommand = new CreateUserCommand(
            FirstName: "Jane",
            LastName: "Doe",
            Email: $"jane.doe.{Guid.NewGuid()}@example.com",
            Password: "Test@123");
        
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/users", createCommand);
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Prepare update command
        var updateCommand = new UpdateUserCommand(
            UserId: userId,
            FirstName: "Jane Updated",
            LastName: "Doe Updated",
            Email: $"jane.updated.{Guid.NewGuid()}@example.com");

        // Act
        var response = await authenticatedClient.PutAsJsonAsync($"/api/users/{userId}", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the update
        var getResponse = await authenticatedClient.GetAsync($"/api/users/{userId}");
        var updatedUser = await getResponse.Content.ReadFromJsonAsync<UserDto>();
        updatedUser!.FirstName.Should().Be("Jane Updated");
        updatedUser.LastName.Should().Be("Doe Updated");
    }

    [Fact]
    public async Task GetCurrentUser_WithAuthentication_ShouldReturnCurrentUser()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();

        // Act
        var response = await authenticatedClient.GetAsync("/api/users/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        user.Should().NotBeNull();
        user!.Email.Should().Be("admin@inventoryapp.com");
        user.FirstName.Should().Be("System");
        user.LastName.Should().Be("Administrator");
    }

    [Fact]
    public async Task Activate_WithValidId_ShouldActivateUser()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        
        // Create and deactivate a user first
        var createCommand = new CreateUserCommand(
            FirstName: "Inactive",
            LastName: "User",
            Email: $"inactive.user.{Guid.NewGuid()}@example.com",
            Password: "Test@123");
        
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/users", createCommand);
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Deactivate the user
        await authenticatedClient.PatchAsync($"/api/users/{userId}/deactivate", null);

        // Act - Activate the user
        var response = await authenticatedClient.PatchAsync($"/api/users/{userId}/activate", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify activation
        var getResponse = await authenticatedClient.GetAsync($"/api/users/{userId}");
        var user = await getResponse.Content.ReadFromJsonAsync<UserDto>();
        user!.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Deactivate_WithValidId_ShouldDeactivateUser()
    {
        // Arrange
        var authenticatedClient = await GetAuthenticatedClientAsync();
        
        // Create a user
        var createCommand = new CreateUserCommand(
            FirstName: "Active",
            LastName: "User",
            Email: $"active.user.{Guid.NewGuid()}@example.com",
            Password: "Test@123");
        
        var createResponse = await authenticatedClient.PostAsJsonAsync("/api/users", createCommand);
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // Act - Deactivate the user
        var response = await authenticatedClient.PatchAsync($"/api/users/{userId}/deactivate", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deactivation
        var getResponse = await authenticatedClient.GetAsync($"/api/users/{userId}");
        var user = await getResponse.Content.ReadFromJsonAsync<UserDto>();
        user!.IsActive.Should().BeFalse();
    }
}
