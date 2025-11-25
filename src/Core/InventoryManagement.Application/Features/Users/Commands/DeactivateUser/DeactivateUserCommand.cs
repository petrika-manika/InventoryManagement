using MediatR;

namespace InventoryManagement.Application.Features.Users.Commands.DeactivateUser;

/// <summary>
/// Command to deactivate a user account.
/// Sets the user's IsActive status to false.
/// Any logged-in admin can deactivate any user (including themselves if they want).
/// </summary>
/// <param name="UserId">The ID of the user to deactivate.</param>
public sealed record DeactivateUserCommand(Guid UserId) : IRequest<Unit>;
