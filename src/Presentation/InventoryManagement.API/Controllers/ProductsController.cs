using InventoryManagement.Application.Features.Products.Commands.CreateProduct;
using InventoryManagement.Application.Features.Products.Commands.DeleteProduct;
using InventoryManagement.Application.Features.Products.Commands.UpdateProduct;
using InventoryManagement.Application.Features.Products.Queries.GetAllProducts;
using InventoryManagement.Application.Features.Products.Queries.GetLowStockProducts;
using InventoryManagement.Application.Features.Products.Queries.GetProductById;
using InventoryManagement.Application.Features.Products.Queries.GetProductsByType;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers;

/// <summary>
/// API controller for managing products across all categories.
/// Provides endpoints for CRUD operations on products including Aroma Bombel, Aroma Bottle, 
/// Aroma Device, Sanitizing Device, and Battery products.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR mediator for sending commands and queries.</param>
    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all products from the inventory.
    /// </summary>
    /// <param name="includeInactive">Whether to include inactive/deactivated products. Default is false.</param>
    /// <returns>A list of all products.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
    {
        var query = new GetAllProductsQuery { IncludeInactive = includeInactive };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets products filtered by product type.
    /// </summary>
    /// <param name="productTypeId">The product type ID (1=AromaBombel, 2=AromaBottle, 3=AromaDevice, 4=SanitizingDevice, 5=Battery).</param>
    /// <param name="includeInactive">Whether to include inactive products. Default is false.</param>
    /// <returns>A list of products of the specified type.</returns>
    [HttpGet("type/{productTypeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByType(
        [FromRoute] int productTypeId,
        [FromQuery] bool includeInactive = false)
    {
        var query = new GetProductsByTypeQuery
        {
            ProductTypeId = productTypeId,
            IncludeInactive = includeInactive
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets a specific product by its ID.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <returns>The product details.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var query = new GetProductByIdQuery { ProductId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets products with low stock levels.
    /// </summary>
    /// <param name="threshold">The stock threshold for determining low stock. Default is 10.</param>
    /// <returns>A list of products with stock at or below the threshold.</returns>
    [HttpGet("low-stock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 10)
    {
        var query = new GetLowStockProductsQuery { Threshold = threshold };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new Aroma Bombel product.
    /// </summary>
    /// <param name="command">The command containing product information.</param>
    /// <returns>The created product ID.</returns>
    [HttpPost("aroma-bombel")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAromaBombel([FromBody] CreateAromaBombelCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Creates a new Aroma Bottle product.
    /// </summary>
    /// <param name="command">The command containing product information.</param>
    /// <returns>The created product ID.</returns>
    [HttpPost("aroma-bottle")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAromaBottle([FromBody] CreateAromaBottleCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Creates a new Aroma Device product.
    /// </summary>
    /// <param name="command">The command containing product information.</param>
    /// <returns>The created product ID.</returns>
    [HttpPost("aroma-device")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAromaDevice([FromBody] CreateAromaDeviceCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Creates a new Sanitizing Device product.
    /// </summary>
    /// <param name="command">The command containing product information.</param>
    /// <returns>The created product ID.</returns>
    [HttpPost("sanitizing-device")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateSanitizingDevice([FromBody] CreateSanitizingDeviceCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Creates a new Battery product.
    /// </summary>
    /// <param name="command">The command containing product information.</param>
    /// <returns>The created product ID.</returns>
    [HttpPost("battery")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateBattery([FromBody] CreateBatteryCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing Aroma Bombel product.
    /// </summary>
    /// <param name="id">The product ID from the route.</param>
    /// <param name="command">The command containing updated product information.</param>
    /// <returns>No content on success.</returns>
    [HttpPut("aroma-bombel/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAromaBombel(
        [FromRoute] Guid id,
        [FromBody] UpdateAromaBombelCommand command)
    {
        if (id != command.ProductId)
        {
            return BadRequest("Product ID in route does not match command.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Updates an existing Aroma Bottle product.
    /// </summary>
    /// <param name="id">The product ID from the route.</param>
    /// <param name="command">The command containing updated product information.</param>
    /// <returns>No content on success.</returns>
    [HttpPut("aroma-bottle/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAromaBottle(
        [FromRoute] Guid id,
        [FromBody] UpdateAromaBottleCommand command)
    {
        if (id != command.ProductId)
        {
            return BadRequest("Product ID in route does not match command.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Updates an existing Aroma Device product.
    /// </summary>
    /// <param name="id">The product ID from the route.</param>
    /// <param name="command">The command containing updated product information.</param>
    /// <returns>No content on success.</returns>
    [HttpPut("aroma-device/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAromaDevice(
        [FromRoute] Guid id,
        [FromBody] UpdateAromaDeviceCommand command)
    {
        if (id != command.ProductId)
        {
            return BadRequest("Product ID in route does not match command.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Updates an existing Sanitizing Device product.
    /// </summary>
    /// <param name="id">The product ID from the route.</param>
    /// <param name="command">The command containing updated product information.</param>
    /// <returns>No content on success.</returns>
    [HttpPut("sanitizing-device/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSanitizingDevice(
        [FromRoute] Guid id,
        [FromBody] UpdateSanitizingDeviceCommand command)
    {
        if (id != command.ProductId)
        {
            return BadRequest("Product ID in route does not match command.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Updates an existing Battery product.
    /// </summary>
    /// <param name="id">The product ID from the route.</param>
    /// <param name="command">The command containing updated product information.</param>
    /// <returns>No content on success.</returns>
    [HttpPut("battery/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateBattery(
        [FromRoute] Guid id,
        [FromBody] UpdateBatteryCommand command)
    {
        if (id != command.ProductId)
        {
            return BadRequest("Product ID in route does not match command.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes (deactivates) a product.
    /// This is a soft delete that marks the product as inactive.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <returns>No content on success.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteProductCommand { ProductId = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
