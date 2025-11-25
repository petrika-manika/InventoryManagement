namespace InventoryManagement.Application.Common.Models;

/// <summary>
/// Data Transfer Object for stock history records.
/// Provides a complete audit trail of inventory movements.
/// </summary>
public sealed class StockHistoryDto
{
    /// <summary>
    /// Gets or sets the unique identifier for this stock history record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the product this stock change relates to.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity that was changed.
    /// Positive values indicate additions, negative values indicate removals.
    /// </summary>
    public int QuantityChanged { get; set; }

    /// <summary>
    /// Gets or sets the stock quantity after this change was applied.
    /// </summary>
    public int QuantityAfter { get; set; }

    /// <summary>
    /// Gets or sets the type of change ("Added" or "Removed").
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional reason for this stock change.
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who made this change.
    /// </summary>
    public Guid ChangedBy { get; set; }

    /// <summary>
    /// Gets or sets the full name of the user who made this change.
    /// </summary>
    public string ChangedByName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC timestamp when this change occurred.
    /// </summary>
    public DateTime ChangedAt { get; set; }
}
