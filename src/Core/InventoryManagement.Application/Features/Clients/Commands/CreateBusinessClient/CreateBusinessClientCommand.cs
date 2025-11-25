using MediatR;

namespace InventoryManagement.Application.Features.Clients.Commands.CreateBusinessClient;

/// <summary>
/// Command to create a new business client.
/// </summary>
/// <param name="NIPT">The business NIPT (tax ID).</param>
/// <param name="OwnerFirstName">The business owner's first name.</param>
/// <param name="OwnerLastName">The business owner's last name.</param>
/// <param name="OwnerPhoneNumber">The business owner's phone number.</param>
/// <param name="ContactPersonFirstName">The contact person's first name.</param>
/// <param name="ContactPersonLastName">The contact person's last name.</param>
/// <param name="ContactPersonPhoneNumber">The contact person's phone number.</param>
/// <param name="Address">The business address.</param>
/// <param name="Email">The business email address.</param>
/// <param name="PhoneNumber">The business phone number.</param>
/// <param name="Notes">Additional notes about the business.</param>
public sealed record CreateBusinessClientCommand(
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
    string? Notes) : IRequest<string>;
