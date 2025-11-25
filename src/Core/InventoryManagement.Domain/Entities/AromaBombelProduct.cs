using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

/// <summary>
/// Represents an Aroma Bombel product with taste options.
/// </summary>
public sealed class AromaBombelProduct : Product
{
    /// <summary>
    /// Gets the optional taste type for this aroma bombel.
    /// </summary>
    public TasteType? Taste { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    private AromaBombelProduct() : base()
    {
        // Required by EF Core for materialization
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AromaBombelProduct"/> class.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The optional product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="photoUrl">The optional photo URL.</param>
    /// <param name="taste">The optional taste type.</param>
    private AromaBombelProduct(
        ProductName name,
        string? description,
        Money price,
        string? photoUrl,
        TasteType? taste)
        : base(name, description, ProductType.AromaBombel, price, photoUrl)
    {
        Taste = taste;
    }

    /// <summary>
    /// Creates a new Aroma Bombel product.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The optional product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="photoUrl">The optional photo URL.</param>
    /// <param name="taste">The optional taste type.</param>
    /// <returns>A new instance of <see cref="AromaBombelProduct"/>.</returns>
    public static AromaBombelProduct Create(
        ProductName name,
        string? description,
        Money price,
        string? photoUrl,
        TasteType? taste)
    {
        return new AromaBombelProduct(name, description, price, photoUrl, taste);
    }

    /// <summary>
    /// Updates the specific information for this Aroma Bombel product.
    /// </summary>
    /// <param name="taste">The optional taste type.</param>
    public void UpdateSpecificInfo(TasteType? taste)
    {
        Taste = taste;
        UpdateTimestamp();
    }
}
