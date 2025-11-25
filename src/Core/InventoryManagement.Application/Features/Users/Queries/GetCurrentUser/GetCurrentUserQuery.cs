using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Users.Queries.GetCurrentUser;

/// <summary>
/// Query to retrieve the currently authenticated user's information.
/// Uses ICurrentUserService to get the current user from the HTTP context.
/// Returns a UserDto if authenticated, otherwise throws UnauthorizedAccessException.
/// This is a read-only operation that does not modify state.
/// </summary>
public sealed record GetCurrentUserQuery : IRequest<UserDto>;
