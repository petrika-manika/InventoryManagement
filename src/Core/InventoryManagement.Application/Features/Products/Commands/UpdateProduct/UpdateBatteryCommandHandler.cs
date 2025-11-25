using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Handler for UpdateBatteryCommand.
/// Updates an existing Battery product in the database.
/// </summary>
public sealed class UpdateBatteryCommandHandler : IRequestHandler<UpdateBatteryCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateBatteryCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public UpdateBatteryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the UpdateBatteryCommand request.
    /// </summary>
    /// <param name="request">The command containing updated product information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Unit.Value on successful completion.</returns>
    /// <exception cref="ProductNotFoundException">Thrown when the product is not found.</exception>
    /// <exception cref="DuplicateProductNameException">Thrown when the new name already exists in this category.</exception>
    public async Task<Unit> Handle(UpdateBatteryCommand request, CancellationToken cancellationToken)
    {
        // Find the product
        var product = await _context.Products
            .OfType<BatteryProduct>()
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(request.ProductId);
        }

        // Create new product name value object
        var newProductName = ProductName.Create(request.Name);

        // Check if new name is already taken by another product in the same category (exclude current product)
        var isDuplicate = await _context.Products
            .OfType<BatteryProduct>()
            .AnyAsync(p => p.Name == newProductName && p.Id != request.ProductId, cancellationToken);

        if (isDuplicate)
        {
            throw new DuplicateProductNameException(request.Name, ProductType.Battery);
        }

        // Create Money value object
        var price = Money.Create(request.Price, request.Currency);

        // Update basic product information
        product.UpdateBasicInfo(newProductName, request.Description, price, request.PhotoUrl);

        // Convert size ID to enum and update specific information
        BatterySize? size = request.SizeId.HasValue
            ? (BatterySize)request.SizeId.Value
            : null;

        product.UpdateSpecificInfo(request.Type, size, request.Brand);

        // Save changes
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
