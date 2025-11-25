using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the BusinessClient entity.
/// Configures specific properties for business clients including NIPT value object.
/// </summary>
public sealed class BusinessClientConfiguration : IEntityTypeConfiguration<BusinessClient>
{
    /// <summary>
    /// Configures the BusinessClient entity properties, value objects, and indexes.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<BusinessClient> builder)
    {
        // NIPT value object configuration using conversion
        // Convert NIPT value object to string for database storage
        builder.Property(c => c.NIPT)
            .HasConversion(
                nipt => nipt.Value,              // Convert to database
                value => new NIPT(value))        // Convert from database
            .HasColumnName("NIPT")
            .HasMaxLength(10)
            .IsRequired();

        // Owner properties
        builder.Property(c => c.OwnerFirstName)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(c => c.OwnerLastName)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(c => c.OwnerPhoneNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        // Contact person properties
        builder.Property(c => c.ContactPersonFirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.ContactPersonLastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.ContactPersonPhoneNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        // Computed properties - ignore for database
        builder.Ignore(c => c.OwnerFullName);
        builder.Ignore(c => c.ContactPersonFullName);

        // Unique constraint on NIPT (only for active clients)
        builder.HasIndex(c => c.NIPT)
            .IsUnique()
            .HasDatabaseName("IX_BusinessClients_NIPT_Unique")
            .HasFilter("[IsActive] = 1");

        // Indexes for search performance
        builder.HasIndex(c => c.ContactPersonFirstName)
            .HasDatabaseName("IX_BusinessClients_ContactPersonFirstName");

        builder.HasIndex(c => c.ContactPersonLastName)
            .HasDatabaseName("IX_BusinessClients_ContactPersonLastName");
    }
}
