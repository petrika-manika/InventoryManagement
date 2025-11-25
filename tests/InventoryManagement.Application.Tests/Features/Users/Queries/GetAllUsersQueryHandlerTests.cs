using FluentAssertions;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Features.Users.Queries.GetAllUsers;
using InventoryManagement.Application.Tests.Helpers;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryManagement.Application.Tests.Features.Users.Queries;

/// <summary>
/// Unit tests for GetAllUsersQueryHandler.
/// Tests query logic for retrieving all users with mocked dependencies.
/// </summary>
public class GetAllUsersQueryHandlerTests
{
    private Mock<IApplicationDbContext> _contextMock;
    private Mock<DbSet<User>> _usersDbSetMock;
    private GetAllUsersQueryHandler _handler;

    private void SetupTest(List<User> users)
    {
        _contextMock = new Mock<IApplicationDbContext>();
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

        _handler = new GetAllUsersQueryHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WithUsers_ShouldReturnAllUsers()
    {
        // Arrange
        var alice = User.Create(
            "Alice",
            "Anderson",
            Email.Create("alice@example.com"),
            "hashed_password");

        var bob = User.Create(
            "Bob",
            "Brown",
            Email.Create("bob@example.com"),
            "hashed_password");

        var charlie = User.Create(
            "Charlie",
            "Clark",
            Email.Create("charlie@example.com"),
            "hashed_password");

        var users = new List<User> { bob, alice, charlie };
        SetupTest(users);

        var query = new GetAllUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().ContainSingle(u => u.Email == "alice@example.com");
        result.Should().ContainSingle(u => u.Email == "bob@example.com");
        result.Should().ContainSingle(u => u.Email == "charlie@example.com");

        // Verify ordering by FirstName
        result[0].FirstName.Should().Be("Alice");
        result[1].FirstName.Should().Be("Bob");
        result[2].FirstName.Should().Be("Charlie");
    }

    [Fact]
    public async Task Handle_WithNoUsers_ShouldReturnEmptyList()
    {
        // Arrange
        var users = new List<User>();
        SetupTest(users);

        var query = new GetAllUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
