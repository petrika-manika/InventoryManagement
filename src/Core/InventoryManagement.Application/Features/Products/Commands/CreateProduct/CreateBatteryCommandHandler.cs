using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Handler for CreateBatteryCommand.
/// Creates a new Battery product in the database.
/// </summary>
public sealed class CreateBatteryCommandHandler : IRequestHandler<CreateBatteryCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBatteryCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public CreateBatteryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the CreateBatteryCommand request.
    /// </summary>
    /// <param name="request">The command containing product information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the newly created product.</returns>
    /// <exception cref="DuplicateProductNameException">Thrown when a product with the same name already exists in this category.</exception>
    public async Task<Guid> Handle(CreateBatteryCommand request, CancellationToken cancellationToken)
    {
        // Create value objects
        var productName = ProductName.Create(request.Name);
        var price = Money.Create(request.Price, request.Currency);

        // Check for duplicate product name within the same category
        var isDuplicate = await _context.Products
            .OfType<BatteryProduct>()
            .AnyAsync(p => p.Name == productName, cancellationToken);

        if (isDuplicate)
        {
            throw new DuplicateProductNameException(request.Name, ProductType.Battery);
        }

        // Convert size ID to enum if provided
        BatterySize? size = request.SizeId.HasValue
            ? (BatterySize)request.SizeId.Value
            : null;

        // Create the product
        var product = BatteryProduct.Create(
            productName,
            request.Description,
            price,
            request.PhotoUrl,
            request.Type,
            size,
            request.Brand);

        // Add to database
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
