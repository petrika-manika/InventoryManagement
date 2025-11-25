using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Clients.Commands.UpdateIndividualClient;

/// <summary>
/// Handler for UpdateIndividualClientCommand.
/// Updates an existing individual client in the system.
/// </summary>
public sealed class UpdateIndividualClientCommandHandler : IRequestHandler<UpdateIndividualClientCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateIndividualClientCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="currentUserService">The current user service.</param>
    public UpdateIndividualClientCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Handles the UpdateIndividualClientCommand request.
    /// </summary>
    /// <param name="request">The command containing updated client information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Unit value.</returns>
    public async Task<Unit> Handle(UpdateIndividualClientCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var currentUserId = _currentUserService.UserId?.ToString()
            ?? throw new UnauthorizedAccessException("User must be authenticated to update clients.");

        // Find the individual client
        var client = await _context.Clients
            .OfType<IndividualClient>()
            .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

        if (client == null)
        {
            throw new InvalidClientDataException($"Client with ID '{request.ClientId}' not found.");
        }

        // Update client information (validation happens in entity method)
        client.UpdatePersonalInfo(
            firstName: request.FirstName,
            lastName: request.LastName,
            address: request.Address,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            notes: request.Notes);

        // Save changes
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
