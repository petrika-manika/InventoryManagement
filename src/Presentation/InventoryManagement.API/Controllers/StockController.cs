using InventoryManagement.Application.Features.Products.Commands.AddStock;
using InventoryManagement.Application.Features.Products.Commands.RemoveStock;
using InventoryManagement.Application.Features.Products.Queries.GetStockHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers;

/// <summary>
/// API controller for managing product stock levels and viewing stock history.
/// Provides endpoints for adding stock, removing stock, and querying stock movement history.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="StockController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR mediator for sending commands and queries.</param>
    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Adds stock to a product inventory.
    /// Creates a stock history record for audit purposes.
    /// </summary>
    /// <param name="command">The command containing product ID, quantity to add, and optional reason.</param>
    /// <returns>The product ID and new stock quantity.</returns>
    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddStock([FromBody] AddStockCommand command)
    {
        var newStockQuantity = await _mediator.Send(command);
        return Ok(new
        {
            ProductId = command.ProductId,
            NewStockQuantity = newStockQuantity
        });
    }

    /// <summary>
    /// Removes stock from a product inventory.
    /// Validates sufficient stock is available before removal.
    /// Creates a stock history record for audit purposes.
    /// </summary>
    /// <param name="command">The command containing product ID, quantity to remove, and optional reason.</param>
    /// <returns>The product ID and new stock quantity.</returns>
    [HttpPost("remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
    public async Task<IActionResult> RemoveStock([FromBody] RemoveStockCommand command)
    {
        var newStockQuantity = await _mediator.Send(command);
        return Ok(new
        {
            ProductId = command.ProductId,
            NewStockQuantity = newStockQuantity
        });
    }

    /// <summary>
    /// Gets stock movement history with optional filters.
    /// </summary>
    /// <param name="productId">Optional product ID to filter history for a specific product.</param>
    /// <param name="fromDate">Optional start date for the history range.</param>
    /// <param name="toDate">Optional end date for the history range.</param>
    /// <param name="take">Maximum number of records to return. Default is 50.</param>
    /// <returns>A list of stock history records.</returns>
    [HttpGet("history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(
        [FromQuery] Guid? productId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int take = 50)
    {
        var query = new GetStockHistoryQuery
        {
            ProductId = productId,
            FromDate = fromDate,
            ToDate = toDate,
            Take = take
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets stock movement history for a specific product.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="fromDate">Optional start date for the history range.</param>
    /// <param name="toDate">Optional end date for the history range.</param>
    /// <param name="take">Maximum number of records to return. Default is 50.</param>
    /// <returns>A list of stock history records for the specified product.</returns>
    [HttpGet("history/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductHistory(
        [FromRoute] Guid productId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int take = 50)
    {
        var query = new GetStockHistoryQuery
        {
            ProductId = productId,
            FromDate = fromDate,
            ToDate = toDate,
            Take = take
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
