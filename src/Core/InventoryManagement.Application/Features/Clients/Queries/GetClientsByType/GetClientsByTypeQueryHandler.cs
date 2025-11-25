using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Clients.Queries.GetClientsByType;

/// <summary>
/// Handler for GetClientsByTypeQuery.
/// Retrieves clients filtered by client type.
/// </summary>
public sealed class GetClientsByTypeQueryHandler : IRequestHandler<GetClientsByTypeQuery, List<ClientDto>>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetClientsByTypeQueryHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetClientsByTypeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the GetClientsByTypeQuery request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of ClientDto objects matching the specified type.</returns>
    public async Task<List<ClientDto>> Handle(GetClientsByTypeQuery request, CancellationToken cancellationToken)
    {
        // Convert ClientTypeId to enum
        var clientType = (ClientType)request.ClientTypeId;

        // Query clients filtered by type
        var query = _context.Clients
            .Where(c => c.ClientType == clientType);

        // Filter by active status if needed
        if (!request.IncludeInactive)
        {
            query = query.Where(c => c.IsActive);
        }

        // Order by CreatedAt descending (newest first)
        var clients = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

        // Map to appropriate DTOs using centralized mapper
        return clients.Select(ClientMapper.MapToDto).ToList();
    }
}
