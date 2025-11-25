using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Features.Clients.Commands.CreateIndividualClient;

/// <summary>
/// Handler for CreateIndividualClientCommand.
/// Creates a new individual client in the system.
/// </summary>
public sealed class CreateIndividualClientCommandHandler : IRequestHandler<CreateIndividualClientCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateIndividualClientCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="currentUserService">The current user service.</param>
    public CreateIndividualClientCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Handles the CreateIndividualClientCommand request.
    /// </summary>
    /// <param name="request">The command containing client information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the newly created client.</returns>
    public async Task<string> Handle(CreateIndividualClientCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var currentUserId = _currentUserService.UserId?.ToString() 
            ?? throw new UnauthorizedAccessException("User must be authenticated to create clients.");

        // Create new individual client (validation happens in entity constructor)
        var client = IndividualClient.Create(
            firstName: request.FirstName,
            lastName: request.LastName,
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
