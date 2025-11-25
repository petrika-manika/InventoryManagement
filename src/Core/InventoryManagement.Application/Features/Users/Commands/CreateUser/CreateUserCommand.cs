using MediatR;

namespace InventoryManagement.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Command to create a new user in the system.
/// Returns the ID of the newly created user.
/// </summary>
/// <param name="FirstName">The user's first name.</param>
/// <param name="LastName">The user's last name.</param>
/// <param name="Email">The user's email address.</param>
/// <param name="Password">The user's password (will be hashed before storage).</param>
public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<Guid>;
