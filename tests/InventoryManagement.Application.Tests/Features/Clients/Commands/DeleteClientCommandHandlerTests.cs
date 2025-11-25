using FluentAssertions;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Features.Clients.Commands.DeleteClient;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryManagement.Application.Tests.Features.Clients.Commands;

/// <summary>
/// Unit tests for DeleteClientCommandHandler.
/// </summary>
public sealed class DeleteClientCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<DbSet<Client>> _clientsDbSetMock;
    private readonly DeleteClientCommandHandler _handler;
    private readonly List<Client> _clients;

    public DeleteClientCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _clientsDbSetMock = new Mock<DbSet<Client>>();
        _clients = new List<Client>();

        var queryable = _clients.AsQueryable();
        _clientsDbSetMock.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(queryable.Provider);
        _clientsDbSetMock.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(queryable.Expression);
        _clientsDbSetMock.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        _clientsDbSetMock.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        _contextMock.Setup(x => x.Clients).Returns(_clientsDbSetMock.Object);
        _currentUserServiceMock.Setup(x => x.UserId).Returns(Guid.NewGuid());

        _handler = new DeleteClientCommandHandler(
            _contextMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldSoftDeleteClient()
    {
        // Arrange
        var clientId = Guid.NewGuid().ToString();
        var existingClient = IndividualClient.Create(
            firstName: "John",
            lastName: "Doe",
            address: null,
            email: null,
            phoneNumber: null,
            notes: null,
            createdBy: Guid.NewGuid().ToString());

        typeof(Client).GetProperty("Id")!.SetValue(existingClient, clientId);
        _clients.Add(existingClient);

        var command = new DeleteClientCommand(ClientId: clientId);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        existingClient.IsActive.Should().BeFalse();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithIndividualClient_ShouldSoftDelete()
    {
        // Arrange
        var clientId = Guid.NewGuid().ToString();
        var individualClient = IndividualClient.Create(
            firstName: "John",
            lastName: "Doe",
            address: null,
            email: null,
            phoneNumber: null,
            notes: null,
            createdBy: Guid.NewGuid().ToString());

        typeof(Client).GetProperty("Id")!.SetValue(individualClient, clientId);
        _clients.Add(individualClient);

        var command = new DeleteClientCommand(ClientId: clientId);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        individualClient.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WithBusinessClient_ShouldSoftDelete()
    {
        // Arrange
        var clientId = Guid.NewGuid().ToString();
        var businessClient = BusinessClient.Create(
            nipt: new Domain.ValueObjects.NIPT("K12345678L"),
            contactPersonFirstName: "Jane",
            contactPersonLastName: "Smith",
            ownerFirstName: null,
            ownerLastName: null,
            ownerPhoneNumber: null,
            contactPersonPhoneNumber: null,
            address: null,
            email: null,
            phoneNumber: null,
            notes: null,
            createdBy: Guid.NewGuid().ToString());

        typeof(Client).GetProperty("Id")!.SetValue(businessClient, clientId);
        _clients.Add(businessClient);

        var command = new DeleteClientCommand(ClientId: clientId);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        businessClient.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WithNonExistentClient_ShouldThrowNotFoundException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid().ToString();
        var command = new DeleteClientCommand(ClientId: nonExistentId);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidClientDataException>()
            .WithMessage($"*{nonExistentId}*");
    }

    [Fact]
    public async Task Handle_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var clientId = Guid.NewGuid().ToString();
        var existingClient = IndividualClient.Create(
            firstName: "John",
            lastName: "Doe",
            address: null,
            email: null,
            phoneNumber: null,
            notes: null,
            createdBy: Guid.NewGuid().ToString());

        typeof(Client).GetProperty("Id")!.SetValue(existingClient, clientId);
        var originalUpdatedAt = existingClient.UpdatedAt;

        _clients.Add(existingClient);

        // Wait a bit to ensure time difference
        await Task.Delay(10);

        var command = new DeleteClientCommand(ClientId: clientId);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        existingClient.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public async Task Handle_ShouldNotRemoveFromDatabase()
    {
        // Arrange
        var clientId = Guid.NewGuid().ToString();
        var existingClient = IndividualClient.Create(
            firstName: "John",
            lastName: "Doe",
            address: null,
            email: null,
            phoneNumber: null,
            notes: null,
            createdBy: Guid.NewGuid().ToString());

        typeof(Client).GetProperty("Id")!.SetValue(existingClient, clientId);
        _clients.Add(existingClient);

        var command = new DeleteClientCommand(ClientId: clientId);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _clients.Should().Contain(existingClient);
        _clientsDbSetMock.Verify(x => x.Remove(It.IsAny<Client>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithAlreadyDeactivatedClient_ShouldStillWork()
    {
        // Arrange
        var clientId = Guid.NewGuid().ToString();
        var existingClient = IndividualClient.Create(
            firstName: "John",
            lastName: "Doe",
            address: null,
            email: null,
            phoneNumber: null,
            notes: null,
            createdBy: Guid.NewGuid().ToString());

        typeof(Client).GetProperty("Id")!.SetValue(existingClient, clientId);
        existingClient.Deactivate();  // Already deactivated

        _clients.Add(existingClient);

        var command = new DeleteClientCommand(ClientId: clientId);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        existingClient.IsActive.Should().BeFalse();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithoutAuthentication_ShouldThrowUnauthorizedException()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId).Returns((Guid?)null);

        var command = new DeleteClientCommand(ClientId: Guid.NewGuid().ToString());

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*authenticated*");
    }
}
