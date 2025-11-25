using MediatR;

namespace InventoryManagement.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Command to delete (deactivate) a product.
/// This command performs a soft delete by deactivating the product rather than removing it from the database.
/// Works for all product types.
/// </summary>
public sealed record DeleteProductCommand : IRequest<Unit>
{
    /// <summary>
    /// Gets the ID of the product to delete.
    /// </summary>
    public Guid ProductId { get; init; }
}
