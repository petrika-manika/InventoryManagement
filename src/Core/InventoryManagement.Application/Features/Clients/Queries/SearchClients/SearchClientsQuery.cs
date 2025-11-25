using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Clients.Queries.SearchClients;

/// <summary>
/// Query to search clients by name, NIPT, email, or phone number.
/// </summary>
/// <param name="SearchTerm">The search term to match against client information.</param>
/// <param name="ClientTypeId">Optional filter by client type (1=Individual, 2=Business).</param>
/// <param name="IncludeInactive">Whether to include inactive clients. Default is false.</param>
public sealed record SearchClientsQuery(
    string? SearchTerm,
    int? ClientTypeId = null,
    bool IncludeInactive = false) : IRequest<List<ClientDto>>;
