using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

/// <summary>
/// Represents a Battery product with type, size, and brand options.
/// </summary>
public sealed class BatteryProduct : Product
{
    /// <summary>
    /// Gets the optional battery type description.
    /// </summary>
    public string? Type { get; private set; }

    /// <summary>
    /// Gets the optional battery size.
    /// </summary>
    public BatterySize? Size { get; private set; }

    /// <summary>
    /// Gets the optional battery brand name.
    /// </summary>
    public string? Brand { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    private BatteryProduct() : base()
    {
        // Required by EF Core for materialization
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BatteryProduct"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The optional product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="photoUrl">The optional photo URL.</param>
    /// <param name="type">The optional battery type.</param>
    /// <param name="size">The optional battery size.</param>
    /// <param name="brand">The optional battery brand.</param>
    private BatteryProduct(
        ProductName name,
        string? description,
        Money price,
        string? photoUrl,
        string? type,
        BatterySize? size,
        string? brand)
        : base(name, description, ProductType.Battery, price, photoUrl)
    {
        Type = type;
        Size = size;
        Brand = brand;
    }

    /// <summary>
    /// Creates a new Battery product.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The optional product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="photoUrl">The optional photo URL.</param>
    /// <param name="type">The optional battery type.</param>
    /// <param name="size">The optional battery size.</param>
    /// <param name="brand">The optional battery brand.</param>
    /// <returns>A new instance of <see cref="BatteryProduct"/>.</returns>
    public static BatteryProduct Create(
        ProductName name,
        string? description,
        Money price,
        string? photoUrl,
        string? type,
        BatterySize? size,
        string? brand)
    {
        return new BatteryProduct(name, description, price, photoUrl, type, size, brand);
    }

    /// <summary>
    /// Updates the specific information for this Battery product.
    /// </summary>
    /// <param name="type">The optional battery type.</param>
    /// <param name="size">The optional battery size.</param>
    /// <param name="brand">The optional battery brand.</param>
    public void UpdateSpecificInfo(string? type, BatterySize? size, string? brand)
    {
        Type = type;
        Size = size;
        Brand = brand;
        UpdateTimestamp();
    }
}
