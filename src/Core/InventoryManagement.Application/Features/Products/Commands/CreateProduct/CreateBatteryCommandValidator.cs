using FluentValidation;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Validator for CreateBatteryCommand.
/// Ensures all product data meets business rules and constraints.
/// </summary>
public sealed class CreateBatteryCommandValidator : AbstractValidator<CreateBatteryCommand>
{
    public CreateBatteryCommandValidator()
    {
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

        RuleFor(x => x.Type)
            .MaximumLength(100).WithMessage("Battery type cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Type));

        RuleFor(x => x.SizeId)
            .Must(BeAValidBatterySize).WithMessage("Size ID must be a valid BatterySize value (1-2).")
            .When(x => x.SizeId.HasValue);

        RuleFor(x => x.Brand)
            .MaximumLength(100).WithMessage("Brand cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Brand));
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private static bool BeAValidBatterySize(int? sizeId)
    {
        if (!sizeId.HasValue)
            return true;

        return Enum.IsDefined(typeof(BatterySize), sizeId.Value);
    }
}
