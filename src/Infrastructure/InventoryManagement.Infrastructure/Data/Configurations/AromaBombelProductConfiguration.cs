using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the AromaBombelProduct entity.
/// Configures specific properties for Aroma Bombel products within the TPH inheritance.
/// </summary>
public class AromaBombelProductConfiguration : IEntityTypeConfiguration<AromaBombelProduct>
{
    public void Configure(EntityTypeBuilder<AromaBombelProduct> builder)
    {
        // Configure Taste property
        // Store as int (enum value), nullable
        builder.Property(p => p.Taste)
            .HasColumnName("Taste")
            .IsRequired(false);
    }
}
