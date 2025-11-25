using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Users.Queries.GetCurrentUser;

/// <summary>
/// Handles the GetCurrentUserQuery to retrieve the currently authenticated user.
/// Uses ICurrentUserService to access the current user's information from HTTP context.
/// This is a CQRS query - read-only operation that never modifies state.
/// </summary>
public sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetCurrentUserQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Handles the get current user query.
    /// </summary>
    /// <param name="request">The get current user query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>UserDto containing the current authenticated user's information.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when user is not authenticated.</exception>
    /// <exception cref="UserNotFoundException">Thrown when authenticated user is not found in database.</exception>
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        // Check if user is authenticated
        if (!_currentUserService.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // Check if user ID is available
        if (_currentUserService.UserId is null)
        {
            throw new UnauthorizedAccessException("User ID is not available.");
        }

        // Query user by current user ID and project to UserDto
        var user = await _context.Users
            .Where(u => u.Id == _currentUserService.UserId.Value)
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
            throw new UserNotFoundException(_currentUserService.UserId.Value);
        }

        return user;
    }
}
