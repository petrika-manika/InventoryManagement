using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Handles the GetUserByIdQuery to retrieve a specific user by ID.
/// This is a CQRS query - read-only operation that never modifies state.
/// </summary>
public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the get user by ID query.
    /// </summary>
    /// <param name="request">The get user by ID query containing the user ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>UserDto containing the user information.</returns>
    /// <exception cref="UserNotFoundException">Thrown when user with specified ID is not found.</exception>
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        // Query user by ID and project to UserDto
        var user = await _context.Users
            .Where(u => u.Id == request.UserId)
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
            .FirstOrDefaultAsync(cancellationToken);

        // If user not found, throw exception
        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        return user;
    }
}
