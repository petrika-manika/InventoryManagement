using FluentAssertions;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Features.Clients.Commands.CreateBusinessClient;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryManagement.Application.Tests.Features.Clients.Commands;

/// <summary>
/// Unit tests for CreateBusinessClientCommandHandler.
/// </summary>
public sealed class CreateBusinessClientCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<DbSet<Client>> _clientsDbSetMock;
    private readonly CreateBusinessClientCommandHandler _handler;
    private readonly List<Client> _clients;

    public CreateBusinessClientCommandHandlerTests()
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

        _handler = new CreateBusinessClientCommandHandler(
            _contextMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateClient()
    {
        // Arrange
        var command = new CreateBusinessClientCommand(
            NIPT: "K12345678L",
            OwnerFirstName: "John",
            OwnerLastName: "Doe",
            OwnerPhoneNumber: "+355 69 111 1111",
            ContactPersonFirstName: "Jane",
            ContactPersonLastName: "Smith",
            ContactPersonPhoneNumber: "+355 69 222 2222",
            Address: "123 Business St",
            Email: "contact@business.com",
            PhoneNumber: "+355 69 333 3333",
            Notes: "Test business");

        _clientsDbSetMock.Setup(x => x.Add(It.IsAny<Client>()));
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrEmpty();
        _clientsDbSetMock.Verify(x => x.Add(It.Is<BusinessClient>(c =>
            c.NIPT.Value == "K12345678L" &&
            c.ContactPersonFirstName == "Jane" &&
            c.ContactPersonLastName == "Smith")), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateNIPT_ShouldThrowDuplicateNIPTException()
    {
        // Arrange
        var existingNipt = "K12345678L";
        var existingClient = BusinessClient.Create(
            nipt: new Domain.ValueObjects.NIPT(existingNipt),
            contactPersonFirstName: "Existing",
            contactPersonLastName: "Contact",
            ownerFirstName: null,
            ownerLastName: null,
            ownerPhoneNumber: null,
            contactPersonPhoneNumber: null,
            address: null,
            email: null,
            phoneNumber: null,
            notes: null,
            createdBy: Guid.NewGuid().ToString());

        _clients.Add(existingClient);

        var command = new CreateBusinessClientCommand(
            NIPT: existingNipt,
            OwnerFirstName: null,
            OwnerLastName: null,
            OwnerPhoneNumber: null,
            ContactPersonFirstName: "New",
            ContactPersonLastName: "Contact",
            ContactPersonPhoneNumber: null,
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DuplicateNIPTException>()
            .WithMessage($"*{existingNipt}*");
    }

    [Fact]
    public async Task Handle_WithInvalidNIPT_ShouldThrowException()
    {
        // Arrange
        var command = new CreateBusinessClientCommand(
            NIPT: "INVALID",  // Too short, should be 10 chars
            OwnerFirstName: null,
            OwnerLastName: null,
            OwnerPhoneNumber: null,
            ContactPersonFirstName: "Jane",
            ContactPersonLastName: "Smith",
            ContactPersonPhoneNumber: null,
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*10*");
    }

    [Fact]
    public async Task Handle_ShouldValidateContactPersonNames()
    {
        // Arrange - Missing contact person first name
        var command = new CreateBusinessClientCommand(
            NIPT: "K12345678L",
            OwnerFirstName: null,
            OwnerLastName: null,
            OwnerPhoneNumber: null,
            ContactPersonFirstName: "",  // Empty - invalid
            ContactPersonLastName: "Smith",
            ContactPersonPhoneNumber: null,
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidClientDataException>()
            .WithMessage("*Contact*");
    }

    [Fact]
    public async Task Handle_WithOptionalOwnerInfo_ShouldCreateClient()
    {
        // Arrange - Owner info is optional
        var command = new CreateBusinessClientCommand(
            NIPT: "K12345678L",
            OwnerFirstName: null,
            OwnerLastName: null,
            OwnerPhoneNumber: null,
            ContactPersonFirstName: "Jane",
            ContactPersonLastName: "Smith",
            ContactPersonPhoneNumber: null,
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
        _clientsDbSetMock.Verify(x => x.Add(It.Is<BusinessClient>(c =>
            c.OwnerFirstName == null &&
            c.OwnerLastName == null &&
            c.OwnerPhoneNumber == null)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldSetCreatedAtAndCreatedBy()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

        var command = new CreateBusinessClientCommand(
            NIPT: "K12345678L",
            OwnerFirstName: null,
            OwnerLastName: null,
            OwnerPhoneNumber: null,
            ContactPersonFirstName: "Jane",
            ContactPersonLastName: "Smith",
            ContactPersonPhoneNumber: null,
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        BusinessClient? capturedClient = null;
        _clientsDbSetMock.Setup(x => x.Add(It.IsAny<Client>()))
            .Callback<Client>(c => capturedClient = c as BusinessClient);
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
    public async Task Handle_WithInvalidContactPhoneNumber_ShouldThrowException()
    {
        // Arrange
        var command = new CreateBusinessClientCommand(
            NIPT: "K12345678L",
            OwnerFirstName: null,
            OwnerLastName: null,
            OwnerPhoneNumber: null,
            ContactPersonFirstName: "Jane",
            ContactPersonLastName: "Smith",
            ContactPersonPhoneNumber: "invalid-!@#",
            Address: null,
            Email: null,
            PhoneNumber: null,
            Notes: null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidClientDataException>()
            .WithMessage("*phone*");
    }

    [Fact]
    public async Task Handle_WithoutAuthentication_ShouldThrowUnauthorizedException()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId).Returns((Guid?)null);

        var command = new CreateBusinessClientCommand(
            NIPT: "K12345678L",
            OwnerFirstName: null,
            OwnerLastName: null,
            OwnerPhoneNumber: null,
            ContactPersonFirstName: "Jane",
            ContactPersonLastName: "Smith",
            ContactPersonPhoneNumber: null,
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
