namespace InventoryManagement.Application.Common.Models;

/// <summary>
/// Data Transfer Object for Aroma Device products.
/// Extends the base ProductDto with device-specific properties including color, format, programs, plug type, and coverage area.
/// </summary>
public sealed class AromaDeviceProductDto : ProductDto
{
    /// <summary>
    /// Gets or sets the device color as a string (enum name).
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the color type enum value.
    /// </summary>
    public int? ColorId { get; set; }

    /// <summary>
    /// Gets or sets the device format description.
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets the device programs information.
    /// </summary>
    public string? Programs { get; set; }

    /// <summary>
    /// Gets or sets the plug type as a string (enum name).
    /// </summary>
    public string PlugType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the plug type enum value.
    /// </summary>
    public int PlugTypeId { get; set; }

    /// <summary>
    /// Gets or sets the square meter coverage area for the device.
    /// </summary>
    public decimal? SquareMeter { get; set; }
}
