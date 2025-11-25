using FluentAssertions;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Features.Clients.Commands.CreateIndividualClient;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryManagement.Application.Tests.Features.Clients.Commands;

/// <summary>
/// Unit tests for CreateIndividualClientCommandHandler.
/// </summary>
public sealed class CreateIndividualClientCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<DbSet<Client>> _clientsDbSetMock;
    private readonly CreateIndividualClientCommandHandler _handler;

    public CreateIndividualClientCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _clientsDbSetMock = new Mock<DbSet<Client>>();

        _contextMock.Setup(x => x.Clients).Returns(_clientsDbSetMock.Object);
        _currentUserServiceMock.Setup(x => x.UserId).Returns(Guid.NewGuid());

        _handler = new CreateIndividualClientCommandHandler(
            _contextMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateClient()
    {
        // Arrange
        var command = new CreateIndividualClientCommand(
            FirstName: "John",
            LastName: "Doe",
            Address: "123 Main St",
            Email: "john.doe@example.com",
            PhoneNumber: "+355 69 123 4567",
            Notes: "Test client");

        _clientsDbSetMock.Setup(x => x.Add(It.IsAny<Client>()));
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrEmpty();
        _clientsDbSetMock.Verify(x => x.Add(It.Is<IndividualClient>(c =>
            c.FirstName == "John" &&
            c.LastName == "Doe" &&
            c.Email == "john.doe@example.com")), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidEmail_ShouldThrowException()
    {
        // Arrange
        var command = new CreateIndividualClientCommand(
            FirstName: "John",
            LastName: "Doe",
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
    public async Task Handle_WithEmptyFirstName_ShouldThrowException()
    {
        // Arrange
        var command = new CreateIndividualClientCommand(
            FirstName: "",
            LastName: "Doe",
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
    public async Task Handle_WithEmptyLastName_ShouldThrowException()
    {
        // Arrange
        var command = new CreateIndividualClientCommand(
            FirstName: "John",
            LastName: "",
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidClientDataException>()
            .WithMessage("*Last name*");
    }

    [Fact]
    public async Task Handle_WithInvalidPhoneNumber_ShouldThrowException()
    {
        // Arrange
        var command = new CreateIndividualClientCommand(
            FirstName: "John",
            LastName: "Doe",
            Address: null,
            Email: null,
            PhoneNumber: "invalid-phone-!@#",
            Notes: null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidClientDataException>()
            .WithMessage("*phone*");
    }

    [Fact]
    public async Task Handle_ShouldSetCreatedAtAndCreatedBy()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

        var command = new CreateIndividualClientCommand(
            FirstName: "John",
            LastName: "Doe",
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        IndividualClient? capturedClient = null;
        _clientsDbSetMock.Setup(x => x.Add(It.IsAny<Client>()))
            .Callback<Client>(c => capturedClient = c as IndividualClient);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedClient.Should().NotBeNull();
        capturedClient!.CreatedBy.Should().Be(userId.ToString());
        capturedClient.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        capturedClient.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithNullOptionalFields_ShouldCreateClient()
    {
        // Arrange
        var command = new CreateIndividualClientCommand(
            FirstName: "John",
            LastName: "Doe",
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        _clientsDbSetMock.Setup(x => x.Add(It.IsAny<Client>()));
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrEmpty();
        _clientsDbSetMock.Verify(x => x.Add(It.Is<IndividualClient>(c =>
            c.FirstName == "John" &&
            c.LastName == "Doe" &&
            c.Address == null &&
            c.Email == null &&
            c.PhoneNumber == null)), Times.Once);
    }

    [Fact]
    public async Task Handle_WithoutAuthentication_ShouldThrowUnauthorizedException()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId).Returns((Guid?)null);

        var command = new CreateIndividualClientCommand(
            FirstName: "John",
            LastName: "Doe",
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
