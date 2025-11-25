using MediatR;

namespace InventoryManagement.Application.Features.Clients.Commands.UpdateBusinessClient;

/// <summary>
/// Command to update an existing business client.
/// </summary>
/// <param name="ClientId">The ID of the client to update.</param>
/// <param name="NIPT">The updated NIPT.</param>
/// <param name="OwnerFirstName">The updated owner first name.</param>
/// <param name="OwnerLastName">The updated owner last name.</param>
/// <param name="OwnerPhoneNumber">The updated owner phone number.</param>
/// <param name="ContactPersonFirstName">The updated contact person first name.</param>
/// <param name="ContactPersonLastName">The updated contact person last name.</param>
/// <param name="ContactPersonPhoneNumber">The updated contact person phone number.</param>
/// <param name="Address">The updated address.</param>
/// <param name="Email">The updated email address.</param>
/// <param name="PhoneNumber">The updated phone number.</param>
/// <param name="Notes">The updated notes.</param>
public sealed record UpdateBusinessClientCommand(
    string ClientId,
    string NIPT,
    string? OwnerFirstName,
    string? OwnerLastName,
    string? OwnerPhoneNumber,
    string ContactPersonFirstName,
    string ContactPersonLastName,
    string? ContactPersonPhoneNumber,
    string? Address,
    string? Email,
    string? PhoneNumber,
    string? Notes) : IRequest<Unit>;
