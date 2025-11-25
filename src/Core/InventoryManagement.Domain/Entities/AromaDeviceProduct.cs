using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

/// <summary>
/// Represents an Aroma Device product with color, format, programs, and plug type options.
/// </summary>
public sealed class AromaDeviceProduct : Product
{
    /// <summary>
    /// Gets the optional color for this aroma device.
    /// </summary>
    public ColorType? Color { get; private set; }

    /// <summary>
    /// Gets the optional format description for this aroma device.
    /// </summary>
    public string? Format { get; private set; }

    /// <summary>
    /// Gets the optional programs information for this aroma device.
    /// </summary>
    public string? Programs { get; private set; }

    /// <summary>
    /// Gets the plug type for this aroma device.
    /// </summary>
    public DevicePlugType PlugType { get; private set; }

    /// <summary>
    /// Gets the optional square meter coverage for this aroma device.
    /// </summary>
    public decimal? SquareMeter { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    private AromaDeviceProduct() : base()
    {
        // Required by EF Core for materialization
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AromaDeviceProduct"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The optional product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="photoUrl">The optional photo URL.</param>
    /// <param name="color">The optional color.</param>
    /// <param name="format">The optional format description.</param>
    /// <param name="programs">The optional programs information.</param>
    /// <param name="plugType">The plug type.</param>
    /// <param name="squareMeter">The optional square meter coverage.</param>
    private AromaDeviceProduct(
        ProductName name,
        string? description,
        Money price,
        string? photoUrl,
        ColorType? color,
        string? format,
        string? programs,
        DevicePlugType plugType,
        decimal? squareMeter)
        : base(name, description, ProductType.AromaDevice, price, photoUrl)
    {
        Color = color;
        Format = format;
        Programs = programs;
        PlugType = plugType;
        SquareMeter = squareMeter;
    }

    /// <summary>
    /// Creates a new Aroma Device product.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The optional product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="photoUrl">The optional photo URL.</param>
    /// <param name="color">The optional color.</param>
    /// <param name="format">The optional format description.</param>
    /// <param name="programs">The optional programs information.</param>
    /// <param name="plugType">The plug type (required).</param>
    /// <param name="squareMeter">The optional square meter coverage.</param>
    /// <returns>A new instance of <see cref="AromaDeviceProduct"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when square meter is negative.</exception>
    public static AromaDeviceProduct Create(
        ProductName name,
        string? description,
        Money price,
        string? photoUrl,
        ColorType? color,
        string? format,
        string? programs,
        DevicePlugType plugType,
        decimal? squareMeter)
    {
        if (squareMeter.HasValue && squareMeter.Value < 0)
        {
            throw new ArgumentException("Square meter coverage cannot be negative.", nameof(squareMeter));
        }

        return new AromaDeviceProduct(name, description, price, photoUrl, color, format, programs, plugType, squareMeter);
    }

    /// <summary>
    /// Updates the specific information for this Aroma Device product.
    /// </summary>
    /// <param name="color">The optional color.</param>
    /// <param name="format">The optional format description.</param>
    /// <param name="programs">The optional programs information.</param>
    /// <param name="plugType">The plug type (required).</param>
    /// <param name="squareMeter">The optional square meter coverage.</param>
    /// <exception cref="ArgumentException">Thrown when square meter is negative.</exception>
    public void UpdateSpecificInfo(
        ColorType? color,
        string? format,
        string? programs,
        DevicePlugType plugType,
        decimal? squareMeter)
    {
        if (squareMeter.HasValue && squareMeter.Value < 0)
        {
            throw new ArgumentException("Square meter coverage cannot be negative.", nameof(squareMeter));
        }

        Color = color;
        Format = format;
        Programs = programs;
        PlugType = plugType;
        SquareMeter = squareMeter;
        UpdateTimestamp();
    }
}
