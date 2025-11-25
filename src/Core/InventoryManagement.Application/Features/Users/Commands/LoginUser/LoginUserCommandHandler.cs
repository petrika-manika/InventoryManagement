using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Application.Common.Models;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Features.Users.Commands.LoginUser;

/// <summary>
/// Handles the LoginUserCommand to authenticate users.
/// Validates credentials, generates JWT token, and returns authentication result.
/// </summary>
public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthenticationResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    /// <summary>
    /// Handles the login user command.
    /// </summary>
    /// <param name="request">The login command containing email and password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Authentication result with user info and JWT token.</returns>
    /// <exception cref="InvalidCredentialsException">Thrown when credentials are invalid or user is inactive.</exception>
    public async Task<AuthenticationResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // Create Email value object from request
        var email = Email.Create(request.Email);

        // Find user by email
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        // If user not found, throw exception
        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        // Check if user is active
        if (!user.IsActive)
        {
            throw new InvalidCredentialsException();
        }

        // Generate JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        // Map user entity to UserDto
        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            Email = user.Email.Value,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        // Return authentication result
        return new AuthenticationResult
        {
            User = userDto,
            Token = token
        };
    }
}
