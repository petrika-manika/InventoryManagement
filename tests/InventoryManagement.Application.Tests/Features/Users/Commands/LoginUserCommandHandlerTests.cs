using FluentAssertions;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Features.Users.Commands.LoginUser;
using InventoryManagement.Application.Tests.Helpers;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryManagement.Application.Tests.Features.Users.Commands;

/// <summary>
/// Unit tests for LoginUserCommandHandler.
/// Tests user authentication logic with mocked dependencies.
/// </summary>
public class LoginUserCommandHandlerTests
{
    private Mock<IApplicationDbContext> _contextMock;
    private Mock<IPasswordHasher> _passwordHasherMock;
    private Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private Mock<DbSet<User>> _usersDbSetMock;
    private LoginUserCommandHandler _handler;

    private void SetupTest(List<User> users)
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _usersDbSetMock = new Mock<DbSet<User>>();

        var testData = new TestAsyncEnumerable<User>(users);
        var queryable = testData as IQueryable<User>;
        
        _usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(queryable.Provider);
        _usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryable.Expression);
        _usersDbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        _usersDbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        _usersDbSetMock.As<IAsyncEnumerable<User>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(testData.GetAsyncEnumerator());

        _contextMock.Setup(x => x.Users).Returns(_usersDbSetMock.Object);

        _handler = new LoginUserCommandHandler(
            _contextMock.Object,
            _passwordHasherMock.Object,
            _jwtTokenGeneratorMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnAuthenticationResult()
    {
        // Arrange
        var command = new LoginUserCommand(
            "test@example.com",
            "password123");

        var user = User.Create(
            "John",
            "Doe",
            Email.Create("test@example.com"),
            "hashed_password");

        var users = new List<User> { user };
        SetupTest(users);

        // Mock password verification to return true
        _passwordHasherMock
            .Setup(x => x.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(true);

        // Mock JWT token generation
        _jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken(user))
            .Returns("test_token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(command.Email.ToLowerInvariant());
        result.User.FirstName.Should().Be("John");
        result.User.LastName.Should().Be("Doe");
        result.Token.Should().Be("test_token");
    }

    [Fact]
    public async Task Handle_WithInvalidEmail_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var command = new LoginUserCommand(
            "nonexistent@example.com",
            "password123");

        var users = new List<User>();
        SetupTest(users);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var command = new LoginUserCommand(
            "test@example.com",
            "wrongpassword");

        var user = User.Create(
            "John",
            "Doe",
            Email.Create("test@example.com"),
            "hashed_password");

        var users = new List<User> { user };
        SetupTest(users);

        // Mock password verification to return false
        _passwordHasherMock
            .Setup(x => x.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async Task Handle_WithInactiveUser_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var command = new LoginUserCommand(
            "test@example.com",
            "password123");

        var user = User.Create(
            "John",
            "Doe",
            Email.Create("test@example.com"),
            "hashed_password");

        // Deactivate the user
        user.Deactivate();

        var users = new List<User> { user };
        SetupTest(users);

        // Mock password verification to return true
        _passwordHasherMock
            .Setup(x => x.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }
}
