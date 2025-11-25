using MediatR;

namespace InventoryManagement.Application.Features.Users.Commands.UpdateUser;

/// <summary>
/// Command to update an existing user's information.
/// Any logged-in user can update their own or other users' information.
/// Returns Unit (void equivalent in MediatR).
/// </summary>
/// <param name="UserId">The ID of the user to update.</param>
/// <param name="FirstName">The user's updated first name.</param>
/// <param name="LastName">The user's updated last name.</param>
/// <param name="Email">The user's updated email address.</param>
public sealed record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email) : IRequest<Unit>;
