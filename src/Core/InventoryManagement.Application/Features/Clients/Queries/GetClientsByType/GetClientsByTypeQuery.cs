using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Clients.Queries.GetClientsByType;

/// <summary>
/// Query to retrieve clients filtered by client type.
/// </summary>
/// <param name="ClientTypeId">The client type ID (1=Individual, 2=Business).</param>
/// <param name="IncludeInactive">Whether to include inactive clients. Default is false.</param>
public sealed record GetClientsByTypeQuery(
    int ClientTypeId,
    bool IncludeInactive = false) : IRequest<List<ClientDto>>;
