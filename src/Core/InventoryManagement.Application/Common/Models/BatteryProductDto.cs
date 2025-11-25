namespace InventoryManagement.Application.Common.Models;

/// <summary>
/// Data Transfer Object for Battery products.
/// Extends the base ProductDto with battery-specific properties including type, size, and brand.
/// </summary>
public sealed class BatteryProductDto : ProductDto
{
    /// <summary>
    /// Gets or sets the battery type description.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the battery size as a string (enum name).
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Gets or sets the battery size enum value.
    /// </summary>
    public int? SizeId { get; set; }

    /// <summary>
    /// Gets or sets the battery brand name.
    /// </summary>
    public string? Brand { get; set; }
}
