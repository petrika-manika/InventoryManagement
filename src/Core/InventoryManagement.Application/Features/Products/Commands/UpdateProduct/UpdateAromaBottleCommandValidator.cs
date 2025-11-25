using FluentValidation;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Validator for UpdateAromaBottleCommand.
/// Ensures all product data meets business rules and constraints.
/// </summary>
public sealed class UpdateAromaBottleCommandValidator : AbstractValidator<UpdateAromaBottleCommand>
{
    public UpdateAromaBottleCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MinimumLength(2).WithMessage("Product name must be at least 2 characters long.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency code must be exactly 3 characters.");

        RuleFor(x => x.PhotoUrl)
            .Must(BeAValidUrl).WithMessage("Photo URL must be a valid URL.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhotoUrl));

        RuleFor(x => x.TasteId)
            .Must(BeAValidTasteType).WithMessage("Taste ID must be a valid TasteType value (1-4).")
            .When(x => x.TasteId.HasValue);
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private static bool BeAValidTasteType(int? tasteId)
    {
        if (!tasteId.HasValue)
            return true;

        return Enum.IsDefined(typeof(TasteType), tasteId.Value);
    }
}
