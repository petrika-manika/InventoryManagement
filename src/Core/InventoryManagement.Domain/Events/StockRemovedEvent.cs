namespace InventoryManagement.Domain.Events;

/// <summary>
/// Event raised when stock is removed from a product.
/// </summary>
public sealed class StockRemovedEvent : IEvent
{
    /// <summary>
    /// Gets the identifier of the product from which stock was removed.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets the quantity that was removed from the stock.
    /// </summary>
    public int QuantityRemoved { get; }

    /// <summary>
    /// Gets the new stock level after the removal.
    /// </summary>
    public int NewStockLevel { get; }

    /// <summary>
    /// Gets the UTC timestamp when this event occurred.
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StockRemovedEvent"/> class.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="quantityRemoved">The quantity that was removed.</param>
    /// <param name="newStockLevel">The new stock level after the removal.</param>
    public StockRemovedEvent(Guid productId, int quantityRemoved, int newStockLevel)
    {
        ProductId = productId;
        QuantityRemoved = quantityRemoved;
        NewStockLevel = newStockLevel;
        OccurredOn = DateTime.UtcNow;
    }
}
