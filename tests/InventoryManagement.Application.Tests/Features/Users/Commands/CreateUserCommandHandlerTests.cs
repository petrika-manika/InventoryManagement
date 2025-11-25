using FluentAssertions;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Features.Users.Commands.CreateUser;
using InventoryManagement.Application.Tests.Helpers;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryManagement.Application.Tests.Features.Users.Commands;

/// <summary>
/// Unit tests for CreateUserCommandHandler.
/// Tests user creation logic with mocked dependencies.
/// </summary>
public class CreateUserCommandHandlerTests
{
    private Mock<IApplicationDbContext> _contextMock;
    private Mock<IPasswordHasher> _passwordHasherMock;
    private Mock<DbSet<User>> _usersDbSetMock;
    private CreateUserCommandHandler _handler;

    private void SetupTest(List<User> users)
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
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

        _handler = new CreateUserCommandHandler(_contextMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateUser()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John",
            "Doe",
            "john.doe@example.com",
            "password123");

        var users = new List<User>();
        SetupTest(users);

        // Mock password hasher to return hashed password
        _passwordHasherMock
            .Setup(x => x.HashPassword(command.Password))
            .Returns("hashed_password");

        // Mock SaveChangesAsync to return 1 (one record saved)
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _usersDbSetMock.Verify(x => x.Add(It.Is<User>(u =>
            u.FirstName == command.FirstName &&
            u.LastName == command.LastName &&
            u.Email.Value == command.Email.ToLowerInvariant() &&
            u.PasswordHash == "hashed_password")), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ShouldThrowDuplicateEmailException()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John",
            "Doe",
            "existing@example.com",
            "password123");

        var existingUser = User.Create(
            "Jane",
            "Doe",
            Email.Create("existing@example.com"),
            "hashed_password");

        var users = new List<User> { existingUser };
        SetupTest(users);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DuplicateEmailException>()
            .WithMessage("*existing@example.com*");
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@test.com")]
    [InlineData("test@")]
    public async Task Handle_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        // Arrange
        var users = new List<User>();
        SetupTest(users);

        var command = new CreateUserCommand(
            "John",
            "Doe",
            invalidEmail,
            "password123");

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
