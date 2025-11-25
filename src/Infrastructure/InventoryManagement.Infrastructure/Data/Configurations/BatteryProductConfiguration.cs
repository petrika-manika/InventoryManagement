using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the BatteryProduct entity.
/// Configures specific properties for Battery products within the TPH inheritance.
/// </summary>
public class BatteryProductConfiguration : IEntityTypeConfiguration<BatteryProduct>
{
    public void Configure(EntityTypeBuilder<BatteryProduct> builder)
    {
        // Configure Type property (battery type text)
        builder.Property(p => p.Type)
            .HasColumnName("BatteryType")
            .HasMaxLength(100)
            .IsRequired(false);

        // Configure Size property
        // Store as int (enum value), nullable
        builder.Property(p => p.Size)
            .HasColumnName("Size")
            .IsRequired(false);

        // Configure Brand property
        builder.Property(p => p.Brand)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}
