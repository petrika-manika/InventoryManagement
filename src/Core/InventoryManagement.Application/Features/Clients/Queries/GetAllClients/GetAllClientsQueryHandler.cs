using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Clients.Queries.GetAllClients;

/// <summary>
/// Handler for GetAllClientsQuery.
/// Retrieves all clients and maps them to their appropriate DTO types.
/// </summary>
public sealed class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, List<ClientDto>>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllClientsQueryHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetAllClientsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the GetAllClientsQuery request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of ClientDto objects.</returns>
    public async Task<List<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        // Query clients
        var query = _context.Clients.AsQueryable();

        // Filter by active status if needed
        if (!request.IncludeInactive)
        {
            query = query.Where(c => c.IsActive);
        }

        // Order by ClientType, then by CreatedAt descending (newest first)
        var clients = await query
            .OrderBy(c => c.ClientType)
            .ThenByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

        // Map to appropriate DTOs using centralized mapper
        return clients.Select(ClientMapper.MapToDto).ToList();
    }
}
