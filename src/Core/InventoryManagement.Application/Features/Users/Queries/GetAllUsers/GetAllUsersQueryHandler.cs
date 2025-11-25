using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Users.Queries.GetAllUsers;

/// <summary>
/// Handles the GetAllUsersQuery to retrieve all users from the database.
/// Returns users ordered by first name, then by last name.
/// This is a CQRS query - read-only operation that never modifies state.
/// </summary>
public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the get all users query.
    /// </summary>
    /// <param name="request">The get all users query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of UserDto containing all users.</returns>
    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        // Query all users, order by FirstName then LastName, and project to UserDto
        var users = await _context.Users
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FullName = u.FullName,
                Email = u.Email.Value,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return users;
    }
}
