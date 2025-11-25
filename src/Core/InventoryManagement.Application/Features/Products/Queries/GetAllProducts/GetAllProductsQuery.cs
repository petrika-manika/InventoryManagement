using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Products.Queries.GetAllProducts;

/// <summary>
/// Query to retrieve all products from the inventory.
/// Returns a list of ProductDto objects.
/// </summary>
public sealed record GetAllProductsQuery : IRequest<List<ProductDto>>
{
    /// <summary>
    /// Gets a value indicating whether to include deactivated products in the results.
    /// Defaults to false (only active products).
    /// </summary>
    public bool IncludeInactive { get; init; } = false;
}
