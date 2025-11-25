using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Handler for UpdateSanitizingDeviceCommand.
/// Updates an existing Sanitizing Device product in the database.
/// </summary>
public sealed class UpdateSanitizingDeviceCommandHandler : IRequestHandler<UpdateSanitizingDeviceCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSanitizingDeviceCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public UpdateSanitizingDeviceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the UpdateSanitizingDeviceCommand request.
    /// </summary>
    /// <param name="request">The command containing updated product information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Unit.Value on successful completion.</returns>
    /// <exception cref="ProductNotFoundException">Thrown when the product is not found.</exception>
    /// <exception cref="DuplicateProductNameException">Thrown when the new name already exists in this category.</exception>
    public async Task<Unit> Handle(UpdateSanitizingDeviceCommand request, CancellationToken cancellationToken)
    {
        // Find the product
        var product = await _context.Products
            .OfType<SanitizingDeviceProduct>()
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(request.ProductId);
        }

        // Create new product name value object
        var newProductName = ProductName.Create(request.Name);

        // Check if new name is already taken by another product in the same category (exclude current product)
        var isDuplicate = await _context.Products
            .OfType<SanitizingDeviceProduct>()
            .AnyAsync(p => p.Name == newProductName && p.Id != request.ProductId, cancellationToken);

        if (isDuplicate)
        {
            throw new DuplicateProductNameException(request.Name, ProductType.SanitizingDevice);
        }

        // Create Money value object
        var price = Money.Create(request.Price, request.Currency);

        // Update basic product information
        product.UpdateBasicInfo(newProductName, request.Description, price, request.PhotoUrl);

        // Convert enums and update specific information
        ColorType? color = request.ColorId.HasValue
            ? (ColorType)request.ColorId.Value
            : null;

        var plugType = (DevicePlugType)request.PlugTypeId;

        product.UpdateSpecificInfo(color, request.Format, request.Programs, plugType);

        // Save changes
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
