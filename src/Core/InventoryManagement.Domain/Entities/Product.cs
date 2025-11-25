using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

/// <summary>
/// Base class for all product types in the inventory system.
/// Provides common properties and behaviors shared across all product categories.
/// </summary>
public abstract class Product
{
    /// <summary>
    /// Gets the unique identifier for the product.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the product name.
    /// </summary>
    public ProductName Name { get; private set; } = null!;

    /// <summary>
    /// Gets the optional product description.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the product category type.
    /// </summary>
    public ProductType ProductType { get; private set; }

    /// <summary>
    /// Gets the product price.
    /// </summary>
    public Money Price { get; private set; } = null!;

    /// <summary>
    /// Gets the optional photo URL for the product.
    /// </summary>
    public string? PhotoUrl { get; private set; }

    /// <summary>
    /// Gets the current stock quantity.
    /// </summary>
    public int StockQuantity { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the product is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp when the product was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    protected Product()
    {
        // Required by EF Core for materialization
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The optional product description.</param>
    /// <param name="productType">The product category type.</param>
    /// <param name="price">The product price.</param>
    /// <param name="photoUrl">The optional photo URL.</param>
    /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
    protected Product(
        ProductName name,
        string? description,
        ProductType productType,
        Money price,
        string? photoUrl)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name), "Product name cannot be null.");
        }

        if (price == null)
        {
            throw new ArgumentNullException(nameof(price), "Product price cannot be null.");
        }

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        ProductType = productType;
        Price = price;
        PhotoUrl = photoUrl;
        StockQuantity = 0;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the basic product information.
    /// </summary>
    /// <param name="name">The new product name.</param>
    /// <param name="description">The new description.</param>
    /// <param name="price">The new price.</param>
    /// <param name="photoUrl">The new photo URL.</param>
    /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
    public void UpdateBasicInfo(ProductName name, string? description, Money price, string? photoUrl)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name), "Product name cannot be null.");
        }

        if (price == null)
        {
            throw new ArgumentNullException(nameof(price), "Product price cannot be null.");
        }

        Name = name;
        Description = description;
        Price = price;
        PhotoUrl = photoUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds stock to the product inventory.
    /// </summary>
    /// <param name="quantity">The quantity to add.</param>
    /// <returns>The new stock quantity.</returns>
    /// <exception cref="ArgumentException">Thrown when quantity is not positive.</exception>
    public int AddStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        StockQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
        return StockQuantity;
    }

    /// <summary>
    /// Removes stock from the product inventory.
    /// </summary>
    /// <param name="quantity">The quantity to remove.</param>
    /// <returns>The new stock quantity.</returns>
    /// <exception cref="ArgumentException">Thrown when quantity is not positive.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is insufficient stock.</exception>
    public int RemoveStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        if (StockQuantity < quantity)
        {
            throw new InvalidOperationException(
                $"Insufficient stock. Available: {StockQuantity}, Requested: {quantity}");
        }

        StockQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
        return StockQuantity;
    }

    /// <summary>
    /// Activates the product, making it available for use.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the product, making it unavailable for use.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Determines whether the product stock is low.
    /// </summary>
    /// <param name="threshold">The stock threshold. Default is 10.</param>
    /// <returns>True if stock quantity is at or below the threshold; otherwise, false.</returns>
    public bool IsLowStock(int threshold = 10)
    {
        return StockQuantity <= threshold;
    }

    /// <summary>
    /// Updates the timestamp when the product was last modified.
    /// This method is available to derived classes to update the timestamp when they modify their specific properties.
    /// </summary>
    protected void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates that the product can be safely deleted.
    /// Business Rule: A product can only be deleted if it has no stock.
    /// </summary>
    /// <exception cref="CannotDeleteProductWithStockException">
    /// Thrown when attempting to delete a product with stock quantity greater than zero.
    /// </exception>
    public void ValidateCanBeDeleted()
    {
        if (StockQuantity > 0)
        {
            throw new CannotDeleteProductWithStockException(
                Name.Value,
                StockQuantity
            );
        }
    }
}
