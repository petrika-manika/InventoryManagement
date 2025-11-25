namespace InventoryManagement.Domain.Events;

/// <summary>
/// Event raised when stock is added to a product.
/// </summary>
public sealed class StockAddedEvent : IEvent
{
    /// <summary>
    /// Gets the identifier of the product that received stock.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets the quantity that was added to the stock.
    /// </summary>
    public int QuantityAdded { get; }

    /// <summary>
    /// Gets the new stock level after the addition.
    /// </summary>
    public int NewStockLevel { get; }

    /// <summary>
    /// Gets the UTC timestamp when this event occurred.
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StockAddedEvent"/> class.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="quantityAdded">The quantity that was added.</param>
    /// <param name="newStockLevel">The new stock level after the addition.</param>
    public StockAddedEvent(Guid productId, int quantityAdded, int newStockLevel)
    {
        ProductId = productId;
        QuantityAdded = quantityAdded;
        NewStockLevel = newStockLevel;
        OccurredOn = DateTime.UtcNow;
    }
}
