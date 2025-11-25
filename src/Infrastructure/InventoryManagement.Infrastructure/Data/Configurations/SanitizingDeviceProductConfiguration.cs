using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the SanitizingDeviceProduct entity.
/// Configures specific properties for Sanitizing Device products within the TPH inheritance.
/// </summary>
public class SanitizingDeviceProductConfiguration : IEntityTypeConfiguration<SanitizingDeviceProduct>
{
    public void Configure(EntityTypeBuilder<SanitizingDeviceProduct> builder)
    {
        // Configure Color property
        // Store as int (enum value), nullable
        builder.Property(p => p.Color)
            .HasColumnName("Color")
            .IsRequired(false);

        // Configure Format property
        builder.Property(p => p.Format)
            .HasMaxLength(200)
            .IsRequired(false);

        // Configure Programs property
        builder.Property(p => p.Programs)
            .HasMaxLength(2000)
            .IsRequired(false);

        // Configure PlugType property
        // Store as int (enum value), required
        builder.Property(p => p.PlugType)
            .HasColumnName("PlugType")
            .IsRequired();
    }
}
