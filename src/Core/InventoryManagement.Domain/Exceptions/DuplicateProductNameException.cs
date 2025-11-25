using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a product with a name that already exists within the same category.
/// Product names must be unique within their product type/category.
/// </summary>
public sealed class DuplicateProductNameException : DomainException
{
    /// <summary>
    /// Gets the product name that is duplicated.
    /// </summary>
    public string ProductName { get; }

    /// <summary>
    /// Gets the product type/category in which the duplicate name exists.
    /// </summary>
    public ProductType ProductType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateProductNameException"/> class.
    /// </summary>
    /// <param name="productName">The product name that is duplicated.</param>
    /// <param name="productType">The product type/category in which the duplicate exists.</param>
    public DuplicateProductNameException(string productName, ProductType productType)
        : base($"A product with name '{productName}' already exists in category '{productType}'.")
    {
        ProductName = productName;
        ProductType = productType;
    }
}
