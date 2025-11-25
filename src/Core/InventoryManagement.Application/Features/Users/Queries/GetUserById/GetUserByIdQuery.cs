using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Query to retrieve a specific user by their ID.
/// Returns a UserDto if found, otherwise throws UserNotFoundException.
/// This is a read-only operation that does not modify state.
/// </summary>
/// <param name="UserId">The unique identifier of the user to retrieve.</param>
public sealed record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;
