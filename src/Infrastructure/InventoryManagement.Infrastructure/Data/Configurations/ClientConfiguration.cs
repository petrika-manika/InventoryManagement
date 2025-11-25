using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Client base entity.
/// Configures Table Per Hierarchy (TPH) inheritance strategy for Individual and Business clients.
/// </summary>
public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    /// <summary>
    /// Configures the Client entity and its TPH discriminator.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        // Table name
        builder.ToTable("Clients");

        // Primary key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasMaxLength(36)
            .IsRequired();

        // Configure TPH discriminator on ClientType
        builder.HasDiscriminator(c => c.ClientType)
            .HasValue<IndividualClient>(ClientType.Individual)
            .HasValue<BusinessClient>(ClientType.Business);

        // ClientType stored as int
        builder.Property(c => c.ClientType)
            .HasConversion<int>()
            .IsRequired();

        // Common properties
        builder.Property(c => c.Address)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(c => c.Email)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(c => c.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(c => c.Notes)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);

        // Audit properties
        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(c => c.UpdatedBy)
            .HasMaxLength(36)
            .IsRequired(false);

        // IsActive with default value
        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes for performance
        builder.HasIndex(c => c.ClientType)
            .HasDatabaseName("IX_Clients_ClientType");

        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_Clients_IsActive");

        builder.HasIndex(c => c.Email)
            .HasDatabaseName("IX_Clients_Email");

        builder.HasIndex(c => c.CreatedAt)
            .HasDatabaseName("IX_Clients_CreatedAt");
    }
}
