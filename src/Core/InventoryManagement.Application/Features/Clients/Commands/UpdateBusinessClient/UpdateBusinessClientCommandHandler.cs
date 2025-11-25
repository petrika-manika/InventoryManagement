using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Clients.Commands.UpdateBusinessClient;

/// <summary>
/// Handler for UpdateBusinessClientCommand.
/// Updates an existing business client in the system.
/// </summary>
public sealed class UpdateBusinessClientCommandHandler : IRequestHandler<UpdateBusinessClientCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateBusinessClientCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="currentUserService">The current user service.</param>
    public UpdateBusinessClientCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Handles the UpdateBusinessClientCommand request.
    /// </summary>
    /// <param name="request">The command containing updated client information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Unit value.</returns>
    public async Task<Unit> Handle(UpdateBusinessClientCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var currentUserId = _currentUserService.UserId?.ToString()
            ?? throw new UnauthorizedAccessException("User must be authenticated to update clients.");

        // Find the business client
        var client = await _context.Clients
            .OfType<BusinessClient>()
            .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

        if (client == null)
        {
            throw new InvalidClientDataException($"Client with ID '{request.ClientId}' not found.");
        }

        // Check if NIPT is changing
        var niptUpperCase = request.NIPT.ToUpperInvariant();
        if (client.NIPT.Value != niptUpperCase)
        {
            // Check if new NIPT already exists on a different client
            var niptExists = await _context.Clients
                .OfType<BusinessClient>()
                .AnyAsync(c => c.Id != request.ClientId && c.NIPT.Value == niptUpperCase, cancellationToken);

            if (niptExists)
            {
                throw new DuplicateNIPTException(request.NIPT);
            }
        }

        // Create new NIPT value object (will validate format)
        var nipt = new NIPT(request.NIPT);

        // Update client information (validation happens in entity method)
        client.UpdateBusinessInfo(
            nipt: nipt,
            ownerFirstName: request.OwnerFirstName,
            ownerLastName: request.OwnerLastName,
            ownerPhoneNumber: request.OwnerPhoneNumber,
            contactPersonFirstName: request.ContactPersonFirstName,
            contactPersonLastName: request.ContactPersonLastName,
            contactPersonPhoneNumber: request.ContactPersonPhoneNumber,
            address: request.Address,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            notes: request.Notes);

        // Save changes
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
