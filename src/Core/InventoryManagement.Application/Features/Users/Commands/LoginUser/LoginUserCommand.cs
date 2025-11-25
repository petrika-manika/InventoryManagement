using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Users.Commands.LoginUser;

/// <summary>
/// Command to authenticate a user with email and password.
/// Returns an authentication result containing user information and JWT token.
/// </summary>
/// <param name="Email">The user's email address.</param>
/// <param name="Password">The user's password.</param>
public sealed record LoginUserCommand(
    string Email,
    string Password) : IRequest<AuthenticationResult>;
