using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the AromaBottleProduct entity.
/// Configures specific properties for Aroma Bottle products within the TPH inheritance.
/// </summary>
public class AromaBottleProductConfiguration : IEntityTypeConfiguration<AromaBottleProduct>
{
    public void Configure(EntityTypeBuilder<AromaBottleProduct> builder)
    {
        // Configure Taste property
        // Store as int (enum value), nullable
        builder.Property(p => p.Taste)
            .HasColumnName("Taste")
            .IsRequired(false);
    }
}
