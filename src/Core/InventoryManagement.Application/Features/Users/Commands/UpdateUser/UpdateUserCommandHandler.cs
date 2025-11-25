using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Users.Commands.UpdateUser;

/// <summary>
/// Handles the UpdateUserCommand to update existing user information.
/// Any logged-in user can update any user's information (no restriction on self-update only).
/// Validates email uniqueness and updates user details.
/// </summary>
public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the update user command.
    /// </summary>
    /// <param name="request">The update user command containing updated user details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Unit (void equivalent in MediatR).</returns>
    /// <exception cref="UserNotFoundException">Thrown when the user to update is not found.</exception>
    /// <exception cref="DuplicateEmailException">Thrown when the new email is already taken by another user.</exception>
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Find the user to update
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        // Create Email value object from request
        var email = Email.Create(request.Email);

        // Check if the new email is already taken by another user
        var emailTaken = await _context.Users
            .AnyAsync(u => u.Email == email && u.Id != request.UserId, cancellationToken);

        if (emailTaken)
        {
            throw new DuplicateEmailException(request.Email);
        }

        // Update user information using domain method
        user.UpdateInformation(request.FirstName, request.LastName, email);

        // Save changes to database
        await _context.SaveChangesAsync(cancellationToken);

        // Return Unit.Value (void equivalent)
        return Unit.Value;
    }
}
