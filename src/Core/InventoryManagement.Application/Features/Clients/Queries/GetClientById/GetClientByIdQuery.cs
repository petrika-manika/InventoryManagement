using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Clients.Queries.GetClientById;

/// <summary>
/// Query to retrieve a specific client by ID.
/// </summary>
/// <param name="ClientId">The unique identifier of the client.</param>
public sealed record GetClientByIdQuery(string ClientId) : IRequest<ClientDto>;
