using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

/// <summary>
/// Represents a Sanitizing Device product with color, format, programs, and plug type.
/// </summary>
public sealed class SanitizingDeviceProduct : Product
{
    /// <summary>
    /// Gets the optional color of the device.
    /// </summary>
    public ColorType? Color { get; private set; }

    /// <summary>
    /// Gets the optional format/model information.
    /// </summary>
    public string? Format { get; private set; }

    /// <summary>
    /// Gets the optional programs description.
    /// </summary>
    public string? Programs { get; private set; }

    /// <summary>
    /// Gets the type of plug for the device.
    /// </summary>
    public DevicePlugType PlugType { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    private SanitizingDeviceProduct() : base()
    {
        // Required by EF Core for materialization
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SanitizingDeviceProduct"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The optional product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="photoUrl">The optional photo URL.</param>
    /// <param name="color">The optional device color.</param>
    /// <param name="format">The optional format/model.</param>
    /// <param name="programs">The optional programs description.</param>
    /// <param name="plugType">The plug type.</param>
    private SanitizingDeviceProduct(
        ProductName name,
        string? description,
        Money price,
        string? photoUrl,
        ColorType? color,
        string? format,
        string? programs,
        DevicePlugType plugType)
        : base(name, description, ProductType.SanitizingDevice, price, photoUrl)
    {
        Color = color;
        Format = format;
        Programs = programs;
        PlugType = plugType;
    }

    /// <summary>
    /// Creates a new Sanitizing Device product.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The optional product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="photoUrl">The optional photo URL.</param>
    /// <param name="color">The optional color.</param>
    /// <param name="format">The optional format description.</param>
    /// <param name="programs">The optional programs information.</param>
    /// <param name="plugType">The plug type (required).</param>
    /// <returns>A new instance of <see cref="SanitizingDeviceProduct"/>.</returns>
    public static SanitizingDeviceProduct Create(
        ProductName name,
        string? description,
        Money price,
        string? photoUrl,
        ColorType? color,
        string? format,
        string? programs,
        DevicePlugType plugType)
    {
        return new SanitizingDeviceProduct(name, description, price, photoUrl, color, format, programs, plugType);
    }

    /// <summary>
    /// Updates the specific information for this Sanitizing Device product.
    /// </summary>
    /// <param name="color">The optional color.</param>
    /// <param name="format">The optional format description.</param>
    /// <param name="programs">The optional programs information.</param>
    /// <param name="plugType">The plug type (required).</param>
    public void UpdateSpecificInfo(
        ColorType? color,
        string? format,
        string? programs,
        DevicePlugType plugType)
    {
        Color = color;
        Format = format;
        Programs = programs;
        PlugType = plugType;
        UpdateTimestamp();
    }
}
