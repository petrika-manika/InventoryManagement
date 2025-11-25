using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Data;

/// <summary>
/// Provides seed data for the application database.
/// Seeds an initial admin user for system bootstrapping and first-time setup.
/// </summary>
public static class ApplicationDbContextSeed
{
    /// <summary>
    /// Seeds the default admin user into the database if it doesn't already exist.
    /// This ensures there is always at least one user account available for initial system access.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="passwordHasher">The password hasher service for hashing the admin password.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedDefaultUserAsync(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        // Create admin email
        var adminEmail = Email.Create("admin@inventoryapp.com");

        // Check if admin user already exists
        var adminExists = await context.Users.AnyAsync(u => u.Email == adminEmail);

        if (!adminExists)
        {
            // Hash the default admin password
            var passwordHash = passwordHasher.HashPassword("Admin@123");

            // Create admin user using factory method
            var adminUser = User.Create(
                firstName: "System",
                lastName: "Administrator",
                email: adminEmail,
                passwordHash: passwordHash);

            // Add admin user to context
            context.Users.Add(adminUser);

            // Save changes to database
            await context.SaveChangesAsync();
        }
    }
}
