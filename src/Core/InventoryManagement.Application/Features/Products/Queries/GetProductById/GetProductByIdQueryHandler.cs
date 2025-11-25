using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// Handler for GetProductByIdQuery.
/// Retrieves a single product and maps it to the appropriate DTO type.
/// </summary>
public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetProductByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the GetProductByIdQuery request.
    /// Retrieves the product and maps it to the appropriate DTO based on its concrete type.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The appropriate ProductDto subtype.</returns>
    /// <exception cref="ProductNotFoundException">Thrown when the product is not found.</exception>
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        // Find the product by ID
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(request.ProductId);
        }

        // Map to appropriate DTO based on concrete type
        return MapToDto(product);
    }

    /// <summary>
    /// Maps a product entity to its appropriate DTO based on the concrete type.
    /// </summary>
    /// <param name="product">The product entity to map.</param>
    /// <returns>The appropriate ProductDto subtype.</returns>
    private static ProductDto MapToDto(Product product)
    {
        return product switch
        {
            AromaBombelProduct aromaBombel => new AromaBombelProductDto
            {
                Id = aromaBombel.Id,
                Name = aromaBombel.Name.Value,
                Description = aromaBombel.Description,
                ProductType = aromaBombel.ProductType.ToString(),
                ProductTypeId = (int)aromaBombel.ProductType,
                Price = aromaBombel.Price.Amount,
                Currency = aromaBombel.Price.Currency,
                PhotoUrl = aromaBombel.PhotoUrl,
                StockQuantity = aromaBombel.StockQuantity,
                IsActive = aromaBombel.IsActive,
                IsLowStock = aromaBombel.IsLowStock(),
                CreatedAt = aromaBombel.CreatedAt,
                UpdatedAt = aromaBombel.UpdatedAt,
                Taste = aromaBombel.Taste?.ToString(),
                TasteId = (int?)aromaBombel.Taste
            },

            AromaBottleProduct aromaBottle => new AromaBottleProductDto
            {
                Id = aromaBottle.Id,
                Name = aromaBottle.Name.Value,
                Description = aromaBottle.Description,
                ProductType = aromaBottle.ProductType.ToString(),
                ProductTypeId = (int)aromaBottle.ProductType,
                Price = aromaBottle.Price.Amount,
                Currency = aromaBottle.Price.Currency,
                PhotoUrl = aromaBottle.PhotoUrl,
                StockQuantity = aromaBottle.StockQuantity,
                IsActive = aromaBottle.IsActive,
                IsLowStock = aromaBottle.IsLowStock(),
                CreatedAt = aromaBottle.CreatedAt,
                UpdatedAt = aromaBottle.UpdatedAt,
                Taste = aromaBottle.Taste?.ToString(),
                TasteId = (int?)aromaBottle.Taste
            },

            AromaDeviceProduct aromaDevice => new AromaDeviceProductDto
            {
                Id = aromaDevice.Id,
                Name = aromaDevice.Name.Value,
                Description = aromaDevice.Description,
                ProductType = aromaDevice.ProductType.ToString(),
                ProductTypeId = (int)aromaDevice.ProductType,
                Price = aromaDevice.Price.Amount,
                Currency = aromaDevice.Price.Currency,
                PhotoUrl = aromaDevice.PhotoUrl,
                StockQuantity = aromaDevice.StockQuantity,
                IsActive = aromaDevice.IsActive,
                IsLowStock = aromaDevice.IsLowStock(),
                CreatedAt = aromaDevice.CreatedAt,
                UpdatedAt = aromaDevice.UpdatedAt,
                Color = aromaDevice.Color?.ToString(),
                ColorId = (int?)aromaDevice.Color,
                Format = aromaDevice.Format,
                Programs = aromaDevice.Programs,
                PlugType = aromaDevice.PlugType.ToString(),
                PlugTypeId = (int)aromaDevice.PlugType,
                SquareMeter = aromaDevice.SquareMeter
            },

            SanitizingDeviceProduct sanitizingDevice => new SanitizingDeviceProductDto
            {
                Id = sanitizingDevice.Id,
                Name = sanitizingDevice.Name.Value,
                Description = sanitizingDevice.Description,
                ProductType = sanitizingDevice.ProductType.ToString(),
                ProductTypeId = (int)sanitizingDevice.ProductType,
                Price = sanitizingDevice.Price.Amount,
                Currency = sanitizingDevice.Price.Currency,
                PhotoUrl = sanitizingDevice.PhotoUrl,
                StockQuantity = sanitizingDevice.StockQuantity,
                IsActive = sanitizingDevice.IsActive,
                IsLowStock = sanitizingDevice.IsLowStock(),
                CreatedAt = sanitizingDevice.CreatedAt,
                UpdatedAt = sanitizingDevice.UpdatedAt,
                Color = sanitizingDevice.Color?.ToString(),
                ColorId = (int?)sanitizingDevice.Color,
                Format = sanitizingDevice.Format,
                Programs = sanitizingDevice.Programs,
                PlugType = sanitizingDevice.PlugType.ToString(),
                PlugTypeId = (int)sanitizingDevice.PlugType
            },

            BatteryProduct battery => new BatteryProductDto
            {
                Id = battery.Id,
                Name = battery.Name.Value,
                Description = battery.Description,
                ProductType = battery.ProductType.ToString(),
                ProductTypeId = (int)battery.ProductType,
                Price = battery.Price.Amount,
                Currency = battery.Price.Currency,
                PhotoUrl = battery.PhotoUrl,
                StockQuantity = battery.StockQuantity,
                IsActive = battery.IsActive,
                IsLowStock = battery.IsLowStock(),
                CreatedAt = battery.CreatedAt,
                UpdatedAt = battery.UpdatedAt,
                Type = battery.Type,
                Size = battery.Size?.ToString(),
                SizeId = (int?)battery.Size,
                Brand = battery.Brand
            },

            _ => new ProductDto
            {
                Id = product.Id,
                Name = product.Name.Value,
                Description = product.Description,
                ProductType = product.ProductType.ToString(),
                ProductTypeId = (int)product.ProductType,
                Price = product.Price.Amount,
                Currency = product.Price.Currency,
                PhotoUrl = product.PhotoUrl,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                IsLowStock = product.IsLowStock(),
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            }
        };
    }
}
