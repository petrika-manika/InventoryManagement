using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Clients.Queries.SearchClients;

/// <summary>
/// Handler for SearchClientsQuery.
/// Searches clients by name, NIPT, email, or phone number.
/// </summary>
public sealed class SearchClientsQueryHandler : IRequestHandler<SearchClientsQuery, List<ClientDto>>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchClientsQueryHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public SearchClientsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the SearchClientsQuery request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of ClientDto objects matching the search criteria.</returns>
    public async Task<List<ClientDto>> Handle(SearchClientsQuery request, CancellationToken cancellationToken)
    {
        // Start with all clients
        var query = _context.Clients.AsQueryable();

        // Filter by active status if needed
        if (!request.IncludeInactive)
        {
            query = query.Where(c => c.IsActive);
        }

        // Filter by ClientType if provided
        if (request.ClientTypeId.HasValue)
        {
            var clientType = (ClientType)request.ClientTypeId.Value;
            query = query.Where(c => c.ClientType == clientType);
        }

        // Apply search filter if SearchTerm is provided
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();

            query = query.Where(c =>
                // Search in common fields
                (c.Email != null && c.Email.ToLower().Contains(searchTerm)) ||
                (c.PhoneNumber != null && c.PhoneNumber.ToLower().Contains(searchTerm)) ||
                // Search in IndividualClient fields
                (c is IndividualClient &&
                    (((IndividualClient)c).FirstName.ToLower().Contains(searchTerm) ||
                     ((IndividualClient)c).LastName.ToLower().Contains(searchTerm))) ||
                // Search in BusinessClient fields
                (c is BusinessClient &&
                    (((BusinessClient)c).NIPT.Value.ToLower().Contains(searchTerm) ||
                     ((BusinessClient)c).ContactPersonFirstName.ToLower().Contains(searchTerm) ||
                     ((BusinessClient)c).ContactPersonLastName.ToLower().Contains(searchTerm) ||
                     (((BusinessClient)c).OwnerFirstName != null && ((BusinessClient)c).OwnerFirstName.ToLower().Contains(searchTerm)) ||
                     (((BusinessClient)c).OwnerLastName != null && ((BusinessClient)c).OwnerLastName.ToLower().Contains(searchTerm)))));
        }

        // Order results: prioritize matches in names, then by created date
        var clients = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

        // Map to appropriate DTOs using centralized mapper
        return clients.Select(ClientMapper.MapToDto).ToList();
    }
}
