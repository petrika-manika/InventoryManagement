using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Handler for CreateSanitizingDeviceCommand.
/// Creates a new Sanitizing Device product in the database.
/// </summary>
public sealed class CreateSanitizingDeviceCommandHandler : IRequestHandler<CreateSanitizingDeviceCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSanitizingDeviceCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public CreateSanitizingDeviceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the CreateSanitizingDeviceCommand request.
    /// </summary>
    /// <param name="request">The command containing product information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the newly created product.</returns>
    /// <exception cref="DuplicateProductNameException">Thrown when a product with the same name already exists in this category.</exception>
    public async Task<Guid> Handle(CreateSanitizingDeviceCommand request, CancellationToken cancellationToken)
    {
        // Create value objects
        var productName = ProductName.Create(request.Name);
        var price = Money.Create(request.Price, request.Currency);

        // Check for duplicate product name within the same category
        var isDuplicate = await _context.Products
            .OfType<SanitizingDeviceProduct>()
            .AnyAsync(p => p.Name == productName, cancellationToken);

        if (isDuplicate)
        {
            throw new DuplicateProductNameException(request.Name, ProductType.SanitizingDevice);
        }

        // Convert enums from int to enum types
        ColorType? color = request.ColorId.HasValue
            ? (ColorType)request.ColorId.Value
            : null;

        var plugType = (DevicePlugType)request.PlugTypeId;

        // Create the product
        var product = SanitizingDeviceProduct.Create(
            productName,
            request.Description,
            price,
            request.PhotoUrl,
            color,
            request.Format,
            request.Programs,
            plugType);

        // Add to database
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
