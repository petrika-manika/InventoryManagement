namespace InventoryManagement.Domain.Exceptions;

/// <summary>
/// Exception thrown when a product is not found in the system.
/// </summary>
public sealed class ProductNotFoundException : DomainException
{
    /// <summary>
    /// Gets the product identifier if the product was searched by ID.
    /// </summary>
    public Guid? ProductId { get; }

    /// <summary>
    /// Gets the product name if the product was searched by name.
    /// </summary>
    public string? ProductName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with a product ID.
    /// </summary>
    /// <param name="productId">The identifier of the product that was not found.</param>
    public ProductNotFoundException(Guid productId)
        : base($"Product with ID '{productId}' was not found.")
    {
        ProductId = productId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with a product name.
    /// </summary>
    /// <param name="productName">The name of the product that was not found.</param>
    public ProductNotFoundException(string productName)
        : base($"Product with name '{productName}' was not found.")
    {
        ProductName = productName;
    }
}
