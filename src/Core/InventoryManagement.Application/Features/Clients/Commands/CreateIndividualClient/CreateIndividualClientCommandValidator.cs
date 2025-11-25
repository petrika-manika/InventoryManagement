using FluentValidation;

namespace InventoryManagement.Application.Features.Clients.Commands.CreateIndividualClient;

/// <summary>
/// Validator for CreateIndividualClientCommand.
/// Ensures all input data is valid before creating an individual client.
/// </summary>
public sealed class CreateIndividualClientCommandValidator : AbstractValidator<CreateIndividualClientCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateIndividualClientCommandValidator"/> class.
    /// </summary>
    public CreateIndividualClientCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(1)
            .WithMessage("First name must be at least 1 character.")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(1)
            .WithMessage("Last name must be at least 1 character.")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters.")
            .Matches(@"^[\d\s\+\-\(\)]+$")
            .WithMessage("Phone number can only contain digits, spaces, +, -, (, ).")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
