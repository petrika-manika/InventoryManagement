using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Products.Queries.GetLowStockProducts;

/// <summary>
/// Query to retrieve products with low stock levels.
/// Returns a list of products where stock quantity is at or below the threshold.
/// </summary>
public sealed record GetLowStockProductsQuery : IRequest<List<ProductDto>>
{
    /// <summary>
    /// Gets the stock threshold for determining low stock.
    /// Defaults to 10 units.
    /// </summary>
    public int Threshold { get; init; } = 10;
}
