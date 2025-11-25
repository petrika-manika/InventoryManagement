using FluentValidation;

namespace InventoryManagement.Application.Features.Clients.Commands.UpdateBusinessClient;

/// <summary>
/// Validator for UpdateBusinessClientCommand.
/// Ensures all input data is valid before updating a business client.
/// </summary>
public sealed class UpdateBusinessClientCommandValidator : AbstractValidator<UpdateBusinessClientCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateBusinessClientCommandValidator"/> class.
    /// </summary>
    public UpdateBusinessClientCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("Client ID is required.");

        RuleFor(x => x.NIPT)
            .NotEmpty()
            .WithMessage("NIPT is required.")
            .Length(10)
            .WithMessage("NIPT must be exactly 10 characters.")
            .Matches(@"^[A-Za-z0-9]{10}$")
            .WithMessage("NIPT must contain only letters and numbers.");

        RuleFor(x => x.OwnerFirstName)
            .MaximumLength(50)
            .WithMessage("Owner first name cannot exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.OwnerFirstName));

        RuleFor(x => x.OwnerLastName)
            .MaximumLength(50)
            .WithMessage("Owner last name cannot exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.OwnerLastName));

        RuleFor(x => x.OwnerPhoneNumber)
            .MaximumLength(20)
            .WithMessage("Owner phone number cannot exceed 20 characters.")
            .Matches(@"^[\d\s\+\-\(\)]+$")
            .WithMessage("Owner phone number can only contain digits, spaces, +, -, (, ).")
            .When(x => !string.IsNullOrWhiteSpace(x.OwnerPhoneNumber));

        RuleFor(x => x.ContactPersonFirstName)
            .NotEmpty()
            .WithMessage("Contact person first name is required.")
            .MinimumLength(1)
            .WithMessage("Contact person first name must be at least 1 character.")
            .MaximumLength(50)
            .WithMessage("Contact person first name cannot exceed 50 characters.");

        RuleFor(x => x.ContactPersonLastName)
            .NotEmpty()
            .WithMessage("Contact person last name is required.")
            .MinimumLength(1)
            .WithMessage("Contact person last name must be at least 1 character.")
            .MaximumLength(50)
            .WithMessage("Contact person last name cannot exceed 50 characters.");

        RuleFor(x => x.ContactPersonPhoneNumber)
            .MaximumLength(20)
            .WithMessage("Contact person phone number cannot exceed 20 characters.")
            .Matches(@"^[\d\s\+\-\(\)]+$")
            .WithMessage("Contact person phone number can only contain digits, spaces, +, -, (, ).")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactPersonPhoneNumber));

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
