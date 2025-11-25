using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Users.Queries.GetAllUsers;

/// <summary>
/// Query to retrieve all users from the system.
/// Returns a list of UserDto containing all user information.
/// This is a read-only operation that does not modify state.
/// </summary>
public sealed record GetAllUsersQuery : IRequest<List<UserDto>>;
