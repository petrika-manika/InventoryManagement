using MediatR;

namespace InventoryManagement.Application.Features.Clients.Commands.CreateIndividualClient;

/// <summary>
/// Command to create a new individual client.
/// </summary>
/// <param name="FirstName">The client's first name.</param>
/// <param name="LastName">The client's last name.</param>
/// <param name="Address">The client's address.</param>
/// <param name="Email">The client's email address.</param>
/// <param name="PhoneNumber">The client's phone number.</param>
/// <param name="Notes">Additional notes about the client.</param>
public sealed record CreateIndividualClientCommand(
    string FirstName,
    string LastName,
    string? Address,
    string? Email,
    string? PhoneNumber,
    string? Notes) : IRequest<string>;
