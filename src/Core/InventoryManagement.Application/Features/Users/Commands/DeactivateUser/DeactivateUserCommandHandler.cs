using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Users.Commands.DeactivateUser;

/// <summary>
/// Handles the DeactivateUserCommand to deactivate a user account.
/// Any logged-in admin can deactivate any user (including themselves if they want).
/// Idempotent operation - if user is already inactive, no error is thrown.
/// </summary>
public sealed class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeactivateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the deactivate user command.
    /// </summary>
    /// <param name="request">The deactivate user command containing user ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Unit (void equivalent in MediatR).</returns>
    /// <exception cref="UserNotFoundException">Thrown when the user is not found.</exception>
    public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        // Find the user to deactivate
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        // Deactivate the user (idempotent - safe to call even if already inactive)
        user.Deactivate();

        // Save changes to database
        await _context.SaveChangesAsync(cancellationToken);

        // Return Unit.Value (void equivalent)
        return Unit.Value;
    }
}
