namespace InventoryManagement.Application.Common.Models;

/// <summary>
/// Data Transfer Object for Aroma Bombel products.
/// Extends the base ProductDto with taste-specific properties.
/// </summary>
public sealed class AromaBombelProductDto : ProductDto
{
    /// <summary>
    /// Gets or sets the taste type as a string (enum name).
    /// </summary>
    public string? Taste { get; set; }

    /// <summary>
    /// Gets or sets the taste type enum value.
    /// </summary>
    public int? TasteId { get; set; }
}
