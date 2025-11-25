using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Clients.Commands.CreateBusinessClient;

/// <summary>
/// Handler for CreateBusinessClientCommand.
/// Creates a new business client in the system.
/// </summary>
public sealed class CreateBusinessClientCommandHandler : IRequestHandler<CreateBusinessClientCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBusinessClientCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="currentUserService">The current user service.</param>
    public CreateBusinessClientCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Handles the CreateBusinessClientCommand request.
    /// </summary>
    /// <param name="request">The command containing client information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the newly created client.</returns>
    public async Task<string> Handle(CreateBusinessClientCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var currentUserId = _currentUserService.UserId?.ToString()
            ?? throw new UnauthorizedAccessException("User must be authenticated to create clients.");

        // Check if NIPT already exists
        var niptExists = await _context.Clients
            .OfType<BusinessClient>()
            .AnyAsync(c => c.NIPT.Value == request.NIPT.ToUpperInvariant(), cancellationToken);

        if (niptExists)
        {
            throw new DuplicateNIPTException(request.NIPT);
        }

        // Create NIPT value object (will validate format)
        var nipt = new NIPT(request.NIPT);

        // Create new business client (validation happens in entity constructor)
        var client = BusinessClient.Create(
            nipt: nipt,
            contactPersonFirstName: request.ContactPersonFirstName,
            contactPersonLastName: request.ContactPersonLastName,
            ownerFirstName: request.OwnerFirstName,
            ownerLastName: request.OwnerLastName,
            ownerPhoneNumber: request.OwnerPhoneNumber,
            contactPersonPhoneNumber: request.ContactPersonPhoneNumber,
            address: request.Address,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            notes: request.Notes,
            createdBy: currentUserId);

        // Add to context and save
        _context.Clients.Add(client);
        await _context.SaveChangesAsync(cancellationToken);

        return client.Id;
    }
}
