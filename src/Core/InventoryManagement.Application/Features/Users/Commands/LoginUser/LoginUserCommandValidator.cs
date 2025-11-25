using FluentValidation;

namespace InventoryManagement.Application.Features.Users.Commands.LoginUser;

/// <summary>
/// Validator for LoginUserCommand using FluentValidation.
/// Ensures email and password meet required criteria before authentication.
/// </summary>
public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters");
    }
}
