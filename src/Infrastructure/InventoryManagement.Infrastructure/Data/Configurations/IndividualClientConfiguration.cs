using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the IndividualClient entity.
/// Configures specific properties for individual (personal) clients.
/// </summary>
public sealed class IndividualClientConfiguration : IEntityTypeConfiguration<IndividualClient>
{
    /// <summary>
    /// Configures the IndividualClient entity properties and indexes.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<IndividualClient> builder)
    {
        // FirstName property
        builder.Property(c => c.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        // LastName property
        builder.Property(c => c.LastName)
            .HasMaxLength(50)
            .IsRequired();

        // FullName is computed property, ignore for database
        builder.Ignore(c => c.FullName);

        // Indexes for search performance
        builder.HasIndex(c => c.FirstName)
            .HasDatabaseName("IX_IndividualClients_FirstName");

        builder.HasIndex(c => c.LastName)
            .HasDatabaseName("IX_IndividualClients_LastName");

        // Composite index for full name searches
        builder.HasIndex(c => new { c.FirstName, c.LastName })
            .HasDatabaseName("IX_IndividualClients_FirstName_LastName");
    }
}
