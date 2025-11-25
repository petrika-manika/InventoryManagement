using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Products.Commands.AddStock;

/// <summary>
/// Handler for AddStockCommand.
/// Adds stock to a product and creates a stock history record for audit trail.
/// </summary>
public sealed class AddStockCommandHandler : IRequestHandler<AddStockCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddStockCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="currentUserService">The current user service.</param>
    public AddStockCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Handles the AddStockCommand request.
    /// Adds stock to the product and creates an audit trail record.
    /// </summary>
    /// <param name="request">The command containing stock addition information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The new stock quantity after the addition.</returns>
    /// <exception cref="ProductNotFoundException">Thrown when the product is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the current user is not authenticated.</exception>
    public async Task<int> Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        // Ensure user is authenticated
        if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException("User must be authenticated to add stock.");
        }

        // Find the product
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(request.ProductId);
        }

        // Add stock to the product
        product.AddStock(request.Quantity);

        // Create stock history record for audit trail
        var stockHistory = StockHistory.CreateAddition(
            productId: request.ProductId,
            quantityAdded: request.Quantity,
            quantityAfter: product.StockQuantity,
            reason: request.Reason,
            changedBy: _currentUserService.UserId.Value);

        // Add stock history to context
        _context.StockHistories.Add(stockHistory);

        // Save all changes
        await _context.SaveChangesAsync(cancellationToken);

        return product.StockQuantity;
    }
}
