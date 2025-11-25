using InventoryManagement.Application.Common.Models;
using MediatR;

namespace InventoryManagement.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// Query to retrieve a single product by its ID.
/// Returns the appropriate ProductDto subtype.
/// </summary>
public sealed record GetProductByIdQuery : IRequest<ProductDto>
{
    /// <summary>
    /// Gets the ID of the product to retrieve.
    /// </summary>
    public Guid ProductId { get; init; }
}
