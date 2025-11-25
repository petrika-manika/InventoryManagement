using InventoryManagement.Application.Common.Models;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Features.Products;

/// <summary>
/// Static mapper class for converting Product entities to their corresponding DTOs.
/// Centralizes all product mapping logic to eliminate duplication across query handlers.
/// </summary>
public static class ProductMapper
{
    /// <summary>
    /// Maps a Product entity to its appropriate DTO based on the concrete type.
    /// Uses pattern matching to determine the correct DTO type.
    /// </summary>
    /// <param name="product">The product entity to map.</param>
    /// <returns>The appropriate ProductDto subtype.</returns>
    public static ProductDto MapToDto(Product product)
    {
        return product switch
        {
            AromaBombelProduct aromaBombel => MapAromaBombel(aromaBombel),
            AromaBottleProduct aromaBottle => MapAromaBottle(aromaBottle),
            AromaDeviceProduct aromaDevice => MapAromaDevice(aromaDevice),
            SanitizingDeviceProduct sanitizingDevice => MapSanitizingDevice(sanitizingDevice),
            BatteryProduct battery => MapBattery(battery),
            _ => MapBase(product)
        };
    }

    /// <summary>
    /// Maps an AromaBombelProduct to AromaBombelProductDto.
    /// </summary>
    /// <param name="product">The Aroma Bombel product to map.</param>
    /// <returns>An AromaBombelProductDto with all properties populated.</returns>
    public static AromaBombelProductDto MapAromaBombel(AromaBombelProduct product)
    {
        var dto = new AromaBombelProductDto
        {
            Taste = product.Taste?.ToString(),
            TasteId = (int?)product.Taste
        };

        MapBaseProperties(product, dto);
        return dto;
    }

    /// <summary>
    /// Maps an AromaBottleProduct to AromaBottleProductDto.
    /// </summary>
    /// <param name="product">The Aroma Bottle product to map.</param>
    /// <returns>An AromaBottleProductDto with all properties populated.</returns>
    public static AromaBottleProductDto MapAromaBottle(AromaBottleProduct product)
    {
        var dto = new AromaBottleProductDto
        {
            Taste = product.Taste?.ToString(),
            TasteId = (int?)product.Taste
        };

        MapBaseProperties(product, dto);
        return dto;
    }

    /// <summary>
    /// Maps an AromaDeviceProduct to AromaDeviceProductDto.
    /// </summary>
    /// <param name="product">The Aroma Device product to map.</param>
    /// <returns>An AromaDeviceProductDto with all properties populated.</returns>
    public static AromaDeviceProductDto MapAromaDevice(AromaDeviceProduct product)
    {
        var dto = new AromaDeviceProductDto
        {
            Color = product.Color?.ToString(),
            ColorId = (int?)product.Color,
            Format = product.Format,
            Programs = product.Programs,
            PlugType = product.PlugType.ToString(),
            PlugTypeId = (int)product.PlugType,
            SquareMeter = product.SquareMeter
        };

        MapBaseProperties(product, dto);
        return dto;
    }

    /// <summary>
    /// Maps a SanitizingDeviceProduct to SanitizingDeviceProductDto.
    /// </summary>
    /// <param name="product">The Sanitizing Device product to map.</param>
    /// <returns>A SanitizingDeviceProductDto with all properties populated.</returns>
    public static SanitizingDeviceProductDto MapSanitizingDevice(SanitizingDeviceProduct product)
    {
        var dto = new SanitizingDeviceProductDto
        {
            Color = product.Color?.ToString(),
            ColorId = (int?)product.Color,
            Format = product.Format,
            Programs = product.Programs,
            PlugType = product.PlugType.ToString(),
            PlugTypeId = (int)product.PlugType
        };

        MapBaseProperties(product, dto);
        return dto;
    }

    /// <summary>
    /// Maps a BatteryProduct to BatteryProductDto.
    /// </summary>
    /// <param name="product">The Battery product to map.</param>
    /// <returns>A BatteryProductDto with all properties populated.</returns>
    public static BatteryProductDto MapBattery(BatteryProduct product)
    {
        var dto = new BatteryProductDto
        {
            Type = product.Type,
            Size = product.Size?.ToString(),
            SizeId = (int?)product.Size,
            Brand = product.Brand
        };

        MapBaseProperties(product, dto);
        return dto;
    }

    /// <summary>
    /// Maps a base Product entity to a base ProductDto.
    /// Used as a fallback for unknown product types.
    /// </summary>
    /// <param name="product">The product entity to map.</param>
    /// <returns>A base ProductDto with common properties populated.</returns>
    private static ProductDto MapBase(Product product)
    {
        var dto = new ProductDto();
        MapBaseProperties(product, dto);
        return dto;
    }

    /// <summary>
    /// Maps all common properties from a Product entity to a ProductDto.
    /// This method populates properties shared across all product types.
    /// </summary>
    /// <param name="product">The source product entity.</param>
    /// <param name="dto">The target ProductDto to populate.</param>
    private static void MapBaseProperties(Product product, ProductDto dto)
    {
        dto.Id = product.Id;
        dto.Name = product.Name.Value;
        dto.Description = product.Description;
        dto.ProductType = product.ProductType.ToString();
        dto.ProductTypeId = (int)product.ProductType;
        dto.Price = product.Price.Amount;
        dto.Currency = product.Price.Currency;
        dto.PhotoUrl = product.PhotoUrl;
        dto.StockQuantity = product.StockQuantity;
        dto.IsActive = product.IsActive;
        dto.IsLowStock = product.IsLowStock();
        dto.CreatedAt = product.CreatedAt;
        dto.UpdatedAt = product.UpdatedAt;
    }
}
