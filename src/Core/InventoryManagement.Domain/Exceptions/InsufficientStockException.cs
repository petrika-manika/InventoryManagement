namespace InventoryManagement.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting to remove more stock than is available for a product.
/// </summary>
public sealed class InsufficientStockException : DomainException
{
    /// <summary>
    /// Gets the identifier of the product with insufficient stock.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets the quantity that was requested.
    /// </summary>
    public int RequestedQuantity { get; }

    /// <summary>
    /// Gets the quantity that is currently available.
    /// </summary>
    public int AvailableQuantity { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InsufficientStockException"/> class.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="requestedQuantity">The quantity that was requested.</param>
    /// <param name="availableQuantity">The quantity that is currently available.</param>
    public InsufficientStockException(Guid productId, int requestedQuantity, int availableQuantity)
        : base($"Insufficient stock for product '{productId}'. Requested: {requestedQuantity}, Available: {availableQuantity}.")
    {
        ProductId = productId;
        RequestedQuantity = requestedQuantity;
        AvailableQuantity = availableQuantity;
    }
}
