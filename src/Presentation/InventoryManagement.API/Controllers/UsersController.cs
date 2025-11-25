using InventoryManagement.Application.Features.Users.Commands.ActivateUser;
using InventoryManagement.Application.Features.Users.Commands.CreateUser;
using InventoryManagement.Application.Features.Users.Commands.DeactivateUser;
using InventoryManagement.Application.Features.Users.Commands.UpdateUser;
using InventoryManagement.Application.Features.Users.Queries.GetAllUsers;
using InventoryManagement.Application.Features.Users.Queries.GetCurrentUser;
using InventoryManagement.Application.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers;

/// <summary>
/// User management endpoints for CRUD operations on user accounts.
/// All endpoints require JWT authentication via the [Authorize] attribute.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator for sending commands and queries.</param>
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all users in the system.
    /// Returns a list of users ordered by first name, then last name.
    /// </summary>
    /// <returns>A list of all users.</returns>
    /// <response code="200">Returns the list of all users.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }

    /// <summary>
    /// Gets a specific user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user with the specified ID.</returns>
    /// <response code="200">Returns the user with the specified ID.</response>
    /// <response code="404">User with the specified ID was not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    /// Gets the currently authenticated user's information.
    /// Extracts user identity from the JWT token in the request.
    /// </summary>
    /// <returns>The current authenticated user's information.</returns>
    /// <response code="200">Returns the current user's information.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await _mediator.Send(new GetCurrentUserQuery());
        return Ok(result);
    }

    /// <summary>
    /// Creates a new user account.
    /// </summary>
    /// <param name="command">The command containing user details and password.</param>
    /// <returns>The ID of the newly created user.</returns>
    /// <response code="201">User created successfully. Returns the new user's ID and location header.</response>
    /// <response code="400">Invalid request data or validation errors.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = userId }, userId);
    }

    /// <summary>
    /// Updates an existing user's information.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="command">The command containing updated user information.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">User updated successfully.</response>
    /// <response code="400">ID mismatch or validation errors.</response>
    /// <response code="404">User with the specified ID was not found.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command)
    {
        if (id != command.UserId)
        {
            return BadRequest("ID mismatch");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Activates a user account, allowing them to log in.
    /// </summary>
    /// <param name="id">The ID of the user to activate.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">User activated successfully.</response>
    /// <response code="404">User with the specified ID was not found.</response>
    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _mediator.Send(new ActivateUserCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Deactivates a user account, preventing them from logging in.
    /// </summary>
    /// <param name="id">The ID of the user to deactivate.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">User deactivated successfully.</response>
    /// <response code="404">User with the specified ID was not found.</response>
    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _mediator.Send(new DeactivateUserCommand(id));
        return NoContent();
    }
}
