using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Handles the CreateUserCommand to register new users in the system.
/// Validates email uniqueness, hashes password, and creates user entity.
/// </summary>
public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Handles the create user command.
    /// </summary>
    /// <param name="request">The create user command containing user details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the newly created user.</returns>
    /// <exception cref="DuplicateEmailException">Thrown when email already exists in the system.</exception>
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Create Email value object from request
        var email = Email.Create(request.Email);

        // Check if email already exists
        var emailExists = await _context.Users
            .AnyAsync(u => u.Email == email, cancellationToken);

        if (emailExists)
        {
            throw new DuplicateEmailException(request.Email);
        }

        // Hash the password
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        // Create user entity using factory method
        var user = User.Create(
            request.FirstName,
            request.LastName,
            email,
            passwordHash);

        // Add user to database context
        _context.Users.Add(user);

        // Save changes to database
        await _context.SaveChangesAsync(cancellationToken);

        // Return the ID of the newly created user
        return user.Id;
    }
}
