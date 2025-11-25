using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Products.Commands.RemoveStock;

/// <summary>
/// Handler for RemoveStockCommand.
/// Removes stock from a product and creates a stock history record for audit trail.
/// Checks for low stock conditions after removal.
/// </summary>
public sealed class RemoveStockCommandHandler : IRequestHandler<RemoveStockCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveStockCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="currentUserService">The current user service.</param>
    public RemoveStockCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Handles the RemoveStockCommand request.
    /// Removes stock from the product and creates an audit trail record.
    /// Checks for low stock conditions after removal.
    /// </summary>
    /// <param name="request">The command containing stock removal information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The new stock quantity after the removal.</returns>
    /// <exception cref="ProductNotFoundException">Thrown when the product is not found.</exception>
    /// <exception cref="InsufficientStockException">Thrown when there is not enough stock to remove.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the current user is not authenticated.</exception>
    public async Task<int> Handle(RemoveStockCommand request, CancellationToken cancellationToken)
    {
        // Ensure user is authenticated
        if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException("User must be authenticated to remove stock.");
        }

        // Find the product
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(request.ProductId);
        }

        // Check if there is sufficient stock before attempting removal
        if (product.StockQuantity < request.Quantity)
        {
            throw new InsufficientStockException(
                product.Id,
                request.Quantity,
                product.StockQuantity);
        }

        // Remove stock from the product
        product.RemoveStock(request.Quantity);

        // Create stock history record for audit trail
        var stockHistory = StockHistory.CreateRemoval(
            productId: request.ProductId,
            quantityRemoved: request.Quantity,
            quantityAfter: product.StockQuantity,
            reason: request.Reason,
            changedBy: _currentUserService.UserId.Value);

        // Add stock history to context
        _context.StockHistories.Add(stockHistory);

        // Check if product is now low on stock (using default threshold of 10)
        // This could be used to raise a LowStockAlertEvent in the future
        var isLowStock = product.IsLowStock(threshold: 10);
        if (isLowStock)
        {
            // TODO: Consider raising LowStockAlertEvent for notification systems
            // For now, this is just a check - event handling can be added later
        }

        // Save all changes
        await _context.SaveChangesAsync(cancellationToken);

        return product.StockQuantity;
    }
}
