using FluentValidation;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Validator for UpdateSanitizingDeviceCommand.
/// Ensures all product data meets business rules and constraints.
/// </summary>
public sealed class UpdateSanitizingDeviceCommandValidator : AbstractValidator<UpdateSanitizingDeviceCommand>
{
    public UpdateSanitizingDeviceCommandValidator()
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

        RuleFor(x => x.ColorId)
            .Must(BeAValidColorType).WithMessage("Color ID must be a valid ColorType value (1-11).")
            .When(x => x.ColorId.HasValue);

        RuleFor(x => x.Format)
            .MaximumLength(200).WithMessage("Format cannot exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Format));

        RuleFor(x => x.Programs)
            .MaximumLength(2000).WithMessage("Programs cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Programs));

        RuleFor(x => x.PlugTypeId)
            .NotEmpty().WithMessage("Plug type is required.")
            .Must(BeAValidDevicePlugType).WithMessage("Plug type ID must be a valid DevicePlugType value (1-2).");
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private static bool BeAValidColorType(int? colorId)
    {
        if (!colorId.HasValue)
            return true;

        return Enum.IsDefined(typeof(ColorType), colorId.Value);
    }

    private static bool BeAValidDevicePlugType(int plugTypeId)
    {
        return Enum.IsDefined(typeof(DevicePlugType), plugTypeId);
    }
}
