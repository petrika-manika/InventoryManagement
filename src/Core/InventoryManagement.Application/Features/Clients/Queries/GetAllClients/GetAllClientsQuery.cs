using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Clients.Queries.GetAllClients;

/// <summary>
/// Query to retrieve all clients from the system.
/// </summary>
/// <param name="IncludeInactive">Whether to include inactive/deactivated clients. Default is false.</param>
public sealed record GetAllClientsQuery(bool IncludeInactive = false) : IRequest<List<ClientDto>>;
