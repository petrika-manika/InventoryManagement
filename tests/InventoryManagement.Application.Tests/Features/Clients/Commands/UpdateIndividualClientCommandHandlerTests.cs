using FluentAssertions;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Features.Clients.Commands.UpdateIndividualClient;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryManagement.Application.Tests.Features.Clients.Commands;

/// <summary>
/// Unit tests for UpdateIndividualClientCommandHandler.
/// </summary>
public sealed class UpdateIndividualClientCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<DbSet<Client>> _clientsDbSetMock;
    private readonly UpdateIndividualClientCommandHandler _handler;
    private readonly List<Client> _clients;

    public UpdateIndividualClientCommandHandlerTests()
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

        _handler = new UpdateIndividualClientCommandHandler(
            _contextMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldUpdateClient()
    {
        // Arrange
        var clientId = Guid.NewGuid().ToString();
        var existingClient = IndividualClient.Create(
            firstName: "John",
            lastName: "Doe",
            address: "123 Old St",
            email: "old@example.com",
            phoneNumber: null,
            notes: null,
            createdBy: Guid.NewGuid().ToString());

        // Use reflection to set the Id
        typeof(Client).GetProperty("Id")!.SetValue(existingClient, clientId);

        _clients.Add(existingClient);

        var command = new UpdateIndividualClientCommand(
            ClientId: clientId,
            FirstName: "Jane",
            LastName: "Smith",
            Address: "456 New St",
            Email: "new@example.com",
            PhoneNumber: "+355 69 123 4567",
            Notes: "Updated notes");

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        existingClient.FirstName.Should().Be("Jane");
        existingClient.LastName.Should().Be("Smith");
        existingClient.Address.Should().Be("456 New St");
        existingClient.Email.Should().Be("new@example.com");
        existingClient.PhoneNumber.Should().Be("+355 69 123 4567");
        existingClient.Notes.Should().Be("Updated notes");
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentClient_ShouldThrowNotFoundException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid().ToString();
        var command = new UpdateIndividualClientCommand(
            ClientId: nonExistentId,
            FirstName: "Jane",
            LastName: "Smith",
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

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

        var command = new UpdateIndividualClientCommand(
            ClientId: clientId,
            FirstName: "Jane",
            LastName: "Smith",
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        existingClient.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public async Task Handle_WithInvalidEmail_ShouldThrowException()
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

        var command = new UpdateIndividualClientCommand(
            ClientId: clientId,
            FirstName: "Jane",
            LastName: "Smith",
            Address: null,
            Email: "invalid-email",
            PhoneNumber: null,
            Notes: null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidClientDataException>()
            .WithMessage("*email*");
    }

    [Fact]
    public async Task Handle_WithInvalidPhoneNumber_ShouldThrowException()
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

        var command = new UpdateIndividualClientCommand(
            ClientId: clientId,
            FirstName: "Jane",
            LastName: "Smith",
            Address: null,
            Email: null,
            PhoneNumber: "invalid-!@#",
            Notes: null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidClientDataException>()
            .WithMessage("*phone*");
    }

    [Fact]
    public async Task Handle_WithEmptyFirstName_ShouldThrowException()
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

        var command = new UpdateIndividualClientCommand(
            ClientId: clientId,
            FirstName: "",  // Invalid
            LastName: "Smith",
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidClientDataException>()
            .WithMessage("*First name*");
    }

    [Fact]
    public async Task Handle_WithoutAuthentication_ShouldThrowUnauthorizedException()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId).Returns((Guid?)null);

        var command = new UpdateIndividualClientCommand(
            ClientId: Guid.NewGuid().ToString(),
            FirstName: "Jane",
            LastName: "Smith",
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*authenticated*");
    }
}
