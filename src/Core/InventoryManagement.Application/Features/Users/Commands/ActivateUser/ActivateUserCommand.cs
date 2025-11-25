using MediatR;

namespace InventoryManagement.Application.Features.Users.Commands.ActivateUser;

/// <summary>
/// Command to activate a user account.
/// Sets the user's IsActive status to true.
/// </summary>
/// <param name="UserId">The ID of the user to activate.</param>
public sealed record ActivateUserCommand(Guid UserId) : IRequest<Unit>;
