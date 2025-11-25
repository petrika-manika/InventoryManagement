using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Validator for DeleteProductCommand.
/// Ensures the product exists and has no stock before deletion.
/// </summary>
public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductCommandValidator"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public DeleteProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.ProductId)
            .MustAsync(ExistAsync)
            .WithMessage("Product with ID '{PropertyValue}' does not exist.");

        RuleFor(x => x.ProductId)
            .MustAsync(HaveNoStockAsync)
            .WithMessage("Cannot delete product with existing stock. Please remove all stock before deleting.");
    }

    /// <summary>
    /// Validates that a product exists.
    /// </summary>
    /// <param name="productId">The product ID to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the product exists; otherwise, false.</returns>
    private async Task<bool> ExistAsync(Guid productId, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AnyAsync(p => p.Id == productId, cancellationToken);
    }

    /// <summary>
    /// Validates that a product has no stock.
    /// </summary>
    /// <param name="productId">The product ID to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the product has no stock or doesn't exist; otherwise, false.</returns>
    private async Task<bool> HaveNoStockAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

        // If product doesn't exist, return true (will be caught by ExistAsync validation)
        if (product == null)
        {
            return true;
        }

        // Product can only be deleted if it has no stock
        return product.StockQuantity == 0;
    }
}
