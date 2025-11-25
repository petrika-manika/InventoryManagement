using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Events;

/// <summary>
/// Event raised when a new product is created in the system.
/// </summary>
public sealed class ProductCreatedEvent : IEvent
{
    /// <summary>
    /// Gets the identifier of the created product.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets the name of the created product.
    /// </summary>
    public string ProductName { get; }

    /// <summary>
    /// Gets the type/category of the created product.
    /// </summary>
    public ProductType ProductType { get; }

    /// <summary>
    /// Gets the UTC timestamp when this event occurred.
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCreatedEvent"/> class.
    /// </summary>
    /// <param name="productId">The identifier of the created product.</param>
    /// <param name="productName">The name of the created product.</param>
    /// <param name="productType">The type/category of the created product.</param>
    public ProductCreatedEvent(Guid productId, string productName, ProductType productType)
    {
        ProductId = productId;
        ProductName = productName;
        ProductType = productType;
        OccurredOn = DateTime.UtcNow;
    }
}
