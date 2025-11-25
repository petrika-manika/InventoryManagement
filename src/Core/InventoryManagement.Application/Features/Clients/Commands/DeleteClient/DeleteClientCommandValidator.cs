using FluentValidation;
using InventoryManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Clients.Commands.DeleteClient;

/// <summary>
/// Validator for DeleteClientCommand.
/// Ensures the client exists before attempting to delete.
/// </summary>
public sealed class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteClientCommandValidator"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public DeleteClientCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("Client ID is required.");

        RuleFor(x => x.ClientId)
            .MustAsync(ExistAsync)
            .WithMessage("Client not found.");
    }

    /// <summary>
    /// Validates that a client exists and is active.
    /// </summary>
    /// <param name="clientId">The client ID to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the client exists and is active; otherwise, false.</returns>
    private async Task<bool> ExistAsync(string clientId, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .AnyAsync(c => c.Id == clientId && c.IsActive, cancellationToken);
    }
}
