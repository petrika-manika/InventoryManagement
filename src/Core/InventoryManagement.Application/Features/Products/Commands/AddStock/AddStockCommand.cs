using MediatR;

namespace InventoryManagement.Application.Features.Products.Commands.AddStock;

/// <summary>
/// Command to add stock to a product.
/// Returns the new stock quantity after the addition.
/// </summary>
public sealed record AddStockCommand : IRequest<int>
{
    /// <summary>
    /// Gets the ID of the product to add stock to.
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Gets the quantity to add to the stock.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Gets the optional reason for adding stock.
    /// </summary>
    public string? Reason { get; init; }
}
