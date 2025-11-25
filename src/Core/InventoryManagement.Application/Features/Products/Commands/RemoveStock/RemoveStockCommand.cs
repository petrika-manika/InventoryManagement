using MediatR;

namespace InventoryManagement.Application.Features.Products.Commands.RemoveStock;

/// <summary>
/// Command to remove stock from a product.
/// Returns the new stock quantity after the removal.
/// </summary>
public sealed record RemoveStockCommand : IRequest<int>
{
    /// <summary>
    /// Gets the ID of the product to remove stock from.
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Gets the quantity to remove from the stock.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Gets the optional reason for removing stock.
    /// </summary>
    public string? Reason { get; init; }
}
