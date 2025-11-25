using InventoryManagement.Application.Features.Users.Commands.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers;

/// <summary>
/// Authentication endpoints for user login and JWT token generation.
/// These endpoints do not require authentication.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator for sending commands and queries.</param>
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Authenticates a user with email and password.
    /// Returns user information and a JWT token for subsequent authenticated requests.
    /// </summary>
    /// <param name="command">The login command containing email and password.</param>
    /// <returns>An authentication result with user details and JWT token.</returns>
    /// <response code="200">Login successful. Returns user information and JWT token.</response>
    /// <response code="401">Invalid credentials or user is inactive.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
