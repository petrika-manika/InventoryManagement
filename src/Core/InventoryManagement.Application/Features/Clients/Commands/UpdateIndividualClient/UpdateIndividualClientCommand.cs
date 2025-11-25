using MediatR;

namespace InventoryManagement.Application.Features.Clients.Commands.UpdateIndividualClient;

/// <summary>
/// Command to update an existing individual client.
/// </summary>
/// <param name="ClientId">The ID of the client to update.</param>
/// <param name="FirstName">The updated first name.</param>
/// <param name="LastName">The updated last name.</param>
/// <param name="Address">The updated address.</param>
/// <param name="Email">The updated email address.</param>
/// <param name="PhoneNumber">The updated phone number.</param>
/// <param name="Notes">The updated notes.</param>
public sealed record UpdateIndividualClientCommand(
    string ClientId,
    string FirstName,
    string LastName,
    string? Address,
    string? Email,
    string? PhoneNumber,
    string? Notes) : IRequest<Unit>;
