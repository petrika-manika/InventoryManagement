using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Handler for DeleteProductCommand.
/// Performs a soft delete by deactivating the product rather than removing it from the database.
/// This preserves data integrity and maintains historical records.
/// </summary>
public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public DeleteProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the DeleteProductCommand request.
    /// Deactivates the product rather than physically deleting it from the database.
    /// Includes a safety check to prevent deletion of products with stock.
    /// </summary>
    /// <param name="request">The command containing the product ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Unit.Value on successful completion.</returns>
    /// <exception cref="ProductNotFoundException">Thrown when the product is not found.</exception>
    /// <exception cref="ValidationException">Thrown when the product has stock.</exception>
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        // Find the product (works for all product types due to polymorphism)
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(request.ProductId);
        }

        // Domain business rule enforcement
        // This is the AUTHORITATIVE check - validator is just optimization
        product.ValidateCanBeDeleted();

        // Soft delete: deactivate the product instead of removing it
        product.Deactivate();

        // Save changes
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
