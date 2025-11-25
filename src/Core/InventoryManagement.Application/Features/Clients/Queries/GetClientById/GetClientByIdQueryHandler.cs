using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Clients.Queries.GetClientById;

/// <summary>
/// Handler for GetClientByIdQuery.
/// Retrieves a specific client by ID and maps to appropriate DTO.
/// </summary>
public sealed class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetClientByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetClientByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the GetClientByIdQuery request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ClientDto object.</returns>
    public async Task<ClientDto> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        // Find the client
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

        if (client == null)
        {
            throw new InvalidClientDataException($"Client with ID '{request.ClientId}' not found.");
        }

        // Map to appropriate DTO using centralized mapper
        return ClientMapper.MapToDto(client);
    }
}
