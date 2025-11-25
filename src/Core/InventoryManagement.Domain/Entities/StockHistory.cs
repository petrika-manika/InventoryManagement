namespace InventoryManagement.Domain.Entities;

/// <summary>
/// Represents a stock history record that tracks all stock movements.
/// This entity provides an audit trail for inventory changes.
/// </summary>
public sealed class StockHistory
{
    /// <summary>
    /// Gets the unique identifier for this stock history record.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the product this stock change relates to.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the quantity that was changed.
    /// Positive values indicate additions, negative values indicate removals.
    /// </summary>
    public int QuantityChanged { get; private set; }

    /// <summary>
    /// Gets the stock quantity after this change was applied.
    /// </summary>
    public int QuantityAfter { get; private set; }

    /// <summary>
    /// Gets the type of change ("Added" or "Removed").
    /// </summary>
    public string ChangeType { get; private set; }

    /// <summary>
    /// Gets the optional reason for this stock change.
    /// </summary>
    public string? Reason { get; private set; }

    /// <summary>
    /// Gets the identifier of the user who made this change.
    /// </summary>
    public Guid ChangedBy { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp when this change occurred.
    /// </summary>
    public DateTime ChangedAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StockHistory"/> class.
    /// </summary>
    private StockHistory()
    {
        ChangeType = string.Empty;
    }

    /// <summary>
    /// Creates a new stock history record for a stock addition.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantityAdded">The quantity that was added (must be positive).</param>
    /// <param name="quantityAfter">The stock quantity after the addition.</param>
    /// <param name="reason">The optional reason for adding stock.</param>
    /// <param name="changedBy">The identifier of the user who made the change.</param>
    /// <returns>A new <see cref="StockHistory"/> instance representing the addition.</returns>
    /// <exception cref="ArgumentException">Thrown when quantityAdded is not positive.</exception>
    public static StockHistory CreateAddition(
        Guid productId,
        int quantityAdded,
        int quantityAfter,
        string? reason,
        Guid changedBy)
    {
        if (quantityAdded <= 0)
        {
            throw new ArgumentException("Quantity added must be positive.", nameof(quantityAdded));
        }

        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }

        if (changedBy == Guid.Empty)
        {
            throw new ArgumentException("Changed by user ID cannot be empty.", nameof(changedBy));
        }

        return new StockHistory
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            QuantityChanged = quantityAdded,
            QuantityAfter = quantityAfter,
            ChangeType = "Added",
            Reason = reason,
            ChangedBy = changedBy,
            ChangedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Creates a new stock history record for a stock removal.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantityRemoved">The quantity that was removed (must be positive).</param>
    /// <param name="quantityAfter">The stock quantity after the removal.</param>
    /// <param name="reason">The optional reason for removing stock.</param>
    /// <param name="changedBy">The identifier of the user who made the change.</param>
    /// <returns>A new <see cref="StockHistory"/> instance representing the removal.</returns>
    /// <exception cref="ArgumentException">Thrown when quantityRemoved is not positive.</exception>
    public static StockHistory CreateRemoval(
        Guid productId,
        int quantityRemoved,
        int quantityAfter,
        string? reason,
        Guid changedBy)
    {
        if (quantityRemoved <= 0)
        {
            throw new ArgumentException("Quantity removed must be positive.", nameof(quantityRemoved));
        }

        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }

        if (changedBy == Guid.Empty)
        {
            throw new ArgumentException("Changed by user ID cannot be empty.", nameof(changedBy));
        }

        return new StockHistory
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            QuantityChanged = -quantityRemoved,
            QuantityAfter = quantityAfter,
            ChangeType = "Removed",
            Reason = reason,
            ChangedBy = changedBy,
            ChangedAt = DateTime.UtcNow
        };
    }
}
