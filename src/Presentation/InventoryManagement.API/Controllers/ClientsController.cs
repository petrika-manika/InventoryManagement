using InventoryManagement.Application.Common.Models;
using InventoryManagement.Application.Features.Clients.Commands.CreateBusinessClient;
using InventoryManagement.Application.Features.Clients.Commands.CreateIndividualClient;
using InventoryManagement.Application.Features.Clients.Commands.DeleteClient;
using InventoryManagement.Application.Features.Clients.Commands.UpdateBusinessClient;
using InventoryManagement.Application.Features.Clients.Commands.UpdateIndividualClient;
using InventoryManagement.Application.Features.Clients.Queries.GetAllClients;
using InventoryManagement.Application.Features.Clients.Queries.GetClientById;
using InventoryManagement.Application.Features.Clients.Queries.GetClientsByType;
using InventoryManagement.Application.Features.Clients.Queries.SearchClients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers;

/// <summary>
/// Controller for managing clients (Individual and Business).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientsController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public ClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all clients.
    /// </summary>
    /// <param name="includeInactive">Whether to include inactive clients.</param>
    /// <returns>A list of all clients.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClientDto>>> GetAll([FromQuery] bool includeInactive = false)
    {
        var query = new GetAllClientsQuery(includeInactive);
        var clients = await _mediator.Send(query);
        return Ok(clients);
    }

    /// <summary>
    /// Gets a client by ID.
    /// </summary>
    /// <param name="id">The client ID.</param>
    /// <returns>The client details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientDto>> GetById(string id)
    {
        var query = new GetClientByIdQuery(id);
        var client = await _mediator.Send(query);
        return Ok(client);
    }

    /// <summary>
    /// Gets clients by type.
    /// </summary>
    /// <param name="clientTypeId">The client type ID (1=Individual, 2=Business).</param>
    /// <param name="includeInactive">Whether to include inactive clients.</param>
    /// <returns>A list of clients of the specified type.</returns>
    [HttpGet("type/{clientTypeId}")]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClientDto>>> GetByType(
        int clientTypeId,
        [FromQuery] bool includeInactive = false)
    {
        var query = new GetClientsByTypeQuery(clientTypeId, includeInactive);
        var clients = await _mediator.Send(query);
        return Ok(clients);
    }

    /// <summary>
    /// Searches clients by name, NIPT, email, or phone number.
    /// </summary>
    /// <param name="searchTerm">The search term.</param>
    /// <param name="clientTypeId">Optional filter by client type.</param>
    /// <param name="includeInactive">Whether to include inactive clients.</param>
    /// <returns>A list of matching clients.</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClientDto>>> Search(
        [FromQuery] string? searchTerm,
        [FromQuery] int? clientTypeId,
        [FromQuery] bool includeInactive = false)
    {
        var query = new SearchClientsQuery(searchTerm, clientTypeId, includeInactive);
        var clients = await _mediator.Send(query);
        return Ok(clients);
    }

    /// <summary>
    /// Creates a new individual client.
    /// </summary>
    /// <param name="command">The create individual client command.</param>
    /// <returns>The ID of the created client.</returns>
    [HttpPost("individual")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> CreateIndividual(
        [FromBody] CreateIndividualClientCommand command)
    {
        var clientId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = clientId }, clientId);
    }

    /// <summary>
    /// Creates a new business client.
    /// </summary>
    /// <param name="command">The create business client command.</param>
    /// <returns>The ID of the created client.</returns>
    [HttpPost("business")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> CreateBusiness(
        [FromBody] CreateBusinessClientCommand command)
    {
        var clientId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = clientId }, clientId);
    }

    /// <summary>
    /// Updates an individual client.
    /// </summary>
    /// <param name="id">The client ID.</param>
    /// <param name="firstName">The client's first name.</param>
    /// <param name="lastName">The client's last name.</param>
    /// <param name="address">The client's address.</param>
    /// <param name="email">The client's email.</param>
    /// <param name="phoneNumber">The client's phone number.</param>
    /// <param name="notes">Additional notes.</param>
    /// <returns>No content.</returns>
    [HttpPut("individual/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateIndividual(
        string id,
        [FromBody] UpdateIndividualClientRequest request)
    {
        var command = new UpdateIndividualClientCommand(
            id,
            request.FirstName,
            request.LastName,
            request.Address,
            request.Email,
            request.PhoneNumber,
            request.Notes);

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Updates a business client.
    /// </summary>
    /// <param name="id">The client ID.</param>
    /// <param name="request">The update request.</param>
    /// <returns>No content.</returns>
    [HttpPut("business/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBusiness(
        string id,
        [FromBody] UpdateBusinessClientRequest request)
    {
        var command = new UpdateBusinessClientCommand(
            id,
            request.NIPT,
            request.OwnerFirstName,
            request.OwnerLastName,
            request.OwnerPhoneNumber,
            request.ContactPersonFirstName,
            request.ContactPersonLastName,
            request.ContactPersonPhoneNumber,
            request.Address,
            request.Email,
            request.PhoneNumber,
            request.Notes);

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a client (soft delete).
    /// </summary>
    /// <param name="id">The client ID.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        var command = new DeleteClientCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}

/// <summary>
/// Request model for updating an individual client.
/// </summary>
public record UpdateIndividualClientRequest(
    string FirstName,
    string LastName,
    string? Address,
    string? Email,
    string? PhoneNumber,
    string? Notes);

/// <summary>
/// Request model for updating a business client.
/// </summary>
public record UpdateBusinessClientRequest(
    string NIPT,
    string? OwnerFirstName,
    string? OwnerLastName,
    string? OwnerPhoneNumber,
    string ContactPersonFirstName,
    string ContactPersonLastName,
    string? ContactPersonPhoneNumber,
    string? Address,
    string? Email,
    string? PhoneNumber,
    string? Notes);
