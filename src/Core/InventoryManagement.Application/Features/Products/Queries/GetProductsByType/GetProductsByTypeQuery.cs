using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Products.Queries.GetProductsByType;

/// <summary>
/// Query to retrieve products filtered by product type.
/// Returns a list of ProductDto objects of the specified type.
/// </summary>
public sealed record GetProductsByTypeQuery : IRequest<List<ProductDto>>
{
    /// <summary>
    /// Gets the product type ID to filter by.
    /// </summary>
    public int ProductTypeId { get; init; }

    /// <summary>
    /// Gets a value indicating whether to include deactivated products in the results.
    /// Defaults to false (only active products).
    /// </summary>
    public bool IncludeInactive { get; init; } = false;
}
