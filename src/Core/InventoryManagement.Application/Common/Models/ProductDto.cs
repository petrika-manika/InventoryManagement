using System.Text.Json.Serialization;

namespace InventoryManagement.Application.Common.Models;

/// <summary>
/// Base Data Transfer Object for product information.
/// Contains common properties shared across all product types.
/// Uses JSON polymorphic serialization to support derived types.
/// </summary>
[JsonDerivedType(typeof(ProductDto), typeDiscriminator: "base")]
[JsonDerivedType(typeof(AromaBombelProductDto), typeDiscriminator: "aromaBombel")]
[JsonDerivedType(typeof(AromaBottleProductDto), typeDiscriminator: "aromaBottle")]
[JsonDerivedType(typeof(AromaDeviceProductDto), typeDiscriminator: "aromaDevice")]
[JsonDerivedType(typeof(SanitizingDeviceProductDto), typeDiscriminator: "sanitizingDevice")]
[JsonDerivedType(typeof(BatteryProductDto), typeDiscriminator: "battery")]
public class ProductDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional product description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the product type as a string (enum name).
    /// </summary>
    public string ProductType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product type enum value.
    /// </summary>
    public int ProductTypeId { get; set; }

    /// <summary>
    /// Gets or sets the product price amount.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the currency code for the price.
    /// </summary>
    public string Currency { get; set; } = "ALL";

    /// <summary>
    /// Gets or sets the optional product photo URL.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the current stock quantity.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product stock is low.
    /// </summary>
    public bool IsLowStock { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the product was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
