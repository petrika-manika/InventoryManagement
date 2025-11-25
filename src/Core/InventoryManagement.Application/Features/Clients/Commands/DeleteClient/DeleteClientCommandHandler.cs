using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Clients.Commands.DeleteClient;

/// <summary>
/// Handler for DeleteClientCommand.
/// Performs a soft delete by deactivating the client.
/// </summary>
public sealed class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteClientCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="currentUserService">The current user service.</param>
    public DeleteClientCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Handles the DeleteClientCommand request.
    /// Performs a soft delete by setting IsActive to false.
    /// </summary>
    /// <param name="request">The command containing the client ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Unit value.</returns>
    public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var currentUserId = _currentUserService.UserId?.ToString()
            ?? throw new UnauthorizedAccessException("User must be authenticated to delete clients.");

        // Find the client
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

        if (client == null)
        {
            throw new InvalidClientDataException($"Client with ID '{request.ClientId}' not found.");
        }

        // Perform soft delete
        client.Deactivate();

        // Save changes
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
