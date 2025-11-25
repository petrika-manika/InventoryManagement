using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Application.Features.Users.Commands.LoginUser;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace InventoryManagement.API.Tests.IntegrationTests;

/// <summary>
/// Integration tests for AuthController endpoints.
/// Tests the complete authentication flow including JWT token generation.
/// </summary>
public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var command = new LoginUserCommand(
            Email: "admin@inventoryapp.com",
            Password: "Admin@123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be("admin@inventoryapp.com");
        result.User.FirstName.Should().Be("System");
        result.User.LastName.Should().Be("Administrator");
        result.User.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var command = new LoginUserCommand(
            Email: "admin@inventoryapp.com",
            Password: "WrongPassword123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new LoginUserCommand(
            Email: "invalid-email",
            Password: "Admin@123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
