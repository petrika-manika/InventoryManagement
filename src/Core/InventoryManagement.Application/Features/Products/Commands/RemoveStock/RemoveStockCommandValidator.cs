using FluentValidation;

namespace InventoryManagement.Application.Features.Products.Commands.RemoveStock;

/// <summary>
/// Validator for RemoveStockCommand.
/// Ensures all stock removal data meets business rules and constraints.
/// </summary>
public sealed class RemoveStockCommandValidator : AbstractValidator<RemoveStockCommand>
{
    public RemoveStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}
