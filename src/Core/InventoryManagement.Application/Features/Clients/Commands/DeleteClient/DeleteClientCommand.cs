using MediatR;

namespace InventoryManagement.Application.Features.Clients.Commands.DeleteClient;

/// <summary>
/// Command to delete (deactivate) a client.
/// This performs a soft delete by setting IsActive to false.
/// </summary>
/// <param name="ClientId">The ID of the client to delete.</param>
public sealed record DeleteClientCommand(string ClientId) : IRequest<Unit>;
