namespace InventoryManagement.Domain.Events;

/// <summary>
/// Event raised when a product's stock level falls below the configured threshold.
/// This event can be used to trigger notifications or automated reordering processes.
/// </summary>
public sealed class LowStockAlertEvent : IEvent
{
    /// <summary>
    /// Gets the identifier of the product with low stock.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets the name of the product with low stock.
    /// </summary>
    public string ProductName { get; }

    /// <summary>
    /// Gets the current stock level.
    /// </summary>
    public int CurrentStock { get; }

    /// <summary>
    /// Gets the threshold that triggered this alert.
    /// </summary>
    public int Threshold { get; }

    /// <summary>
    /// Gets the UTC timestamp when this event occurred.
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LowStockAlertEvent"/> class.
    /// </summary>
    /// <param name="productId">The identifier of the product with low stock.</param>
    /// <param name="productName">The name of the product with low stock.</param>
    /// <param name="currentStock">The current stock level.</param>
    /// <param name="threshold">The threshold that triggered this alert.</param>
    public LowStockAlertEvent(Guid productId, string productName, int currentStock, int threshold)
    {
        ProductId = productId;
        ProductName = productName;
        CurrentStock = currentStock;
        Threshold = threshold;
        OccurredOn = DateTime.UtcNow;
    }
}
