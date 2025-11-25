using MediatR;

namespace InventoryManagement.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Command to create a new Battery product.
/// Returns the newly created product's ID.
/// </summary>
public sealed record CreateBatteryCommand : IRequest<Guid>
{
    /// <summary>
    /// Gets the product name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the optional product description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the product price amount.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Gets the currency code for the price. Defaults to "ALL" (Albanian Lek).
    /// </summary>
    public string Currency { get; init; } = "ALL";

    /// <summary>
    /// Gets the optional product photo URL.
    /// </summary>
    public string? PhotoUrl { get; init; }

    /// <summary>
    /// Gets the optional battery type description.
    /// </summary>
    public string? Type { get; init; }

    /// <summary>
    /// Gets the optional battery size enum value.
    /// </summary>
    public int? SizeId { get; init; }

    /// <summary>
    /// Gets the optional battery brand name.
    /// </summary>
    public string? Brand { get; init; }
}
