using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Users.Commands.ActivateUser;

/// <summary>
/// Handles the ActivateUserCommand to activate a user account.
/// Idempotent operation - if user is already active, no error is thrown.
/// </summary>
public sealed class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ActivateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the activate user command.
    /// </summary>
    /// <param name="request">The activate user command containing user ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Unit (void equivalent in MediatR).</returns>
    /// <exception cref="UserNotFoundException">Thrown when the user is not found.</exception>
    public async Task<Unit> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        // Find the user to activate
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        // Activate the user (idempotent - safe to call even if already active)
        user.Activate();

        // Save changes to database
        await _context.SaveChangesAsync(cancellationToken);

        // Return Unit.Value (void equivalent)
        return Unit.Value;
    }
}
