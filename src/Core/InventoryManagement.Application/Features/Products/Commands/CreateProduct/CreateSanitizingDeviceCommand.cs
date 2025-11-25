using MediatR;

namespace InventoryManagement.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Command to create a new Sanitizing Device product.
/// Returns the newly created product's ID.
/// </summary>
public sealed record CreateSanitizingDeviceCommand : IRequest<Guid>
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
    /// Gets the optional color type enum value.
    /// </summary>
    public int? ColorId { get; init; }

    /// <summary>
    /// Gets the optional device format description.
    /// </summary>
    public string? Format { get; init; }

    /// <summary>
    /// Gets the optional device programs information.
    /// </summary>
    public string? Programs { get; init; }

    /// <summary>
    /// Gets the required plug type enum value.
    /// </summary>
    public int PlugTypeId { get; init; }
}
