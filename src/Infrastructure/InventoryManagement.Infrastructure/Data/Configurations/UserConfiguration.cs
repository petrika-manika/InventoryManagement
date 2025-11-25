using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the User entity using Fluent API.
/// Configures table schema, properties, constraints, indexes, and value object conversions.
/// </summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the User entity for Entity Framework Core.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the User entity.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table name
        builder.ToTable("Users");

        // Primary key
        builder.HasKey(u => u.Id);

        // FirstName property
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        // LastName property
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        // Email property (Value Object conversion)
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasConversion(
                email => email.Value,           // To database
                value => Email.Create(value));  // From database

        // Unique index on Email
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        // PasswordHash property
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        // IsActive property with default value
        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // CreatedAt property
        builder.Property(u => u.CreatedAt)
            .IsRequired();

        // UpdatedAt property
        builder.Property(u => u.UpdatedAt)
            .IsRequired();

        // FullName - computed property, not stored in database
        builder.Ignore(u => u.FullName);
    }
}
