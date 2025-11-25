using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Product entity and its derived types.
/// Implements Table Per Hierarchy (TPH) inheritance strategy where all product types
/// are stored in a single "Products" table with a discriminator column.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table name
        builder.ToTable("Products");

        // Primary key
        builder.HasKey(p => p.Id);

        // Configure TPH (Table Per Hierarchy) discriminator
        // Use the existing ProductType property as the discriminator
        builder.HasDiscriminator(p => p.ProductType)
            .HasValue<AromaBombelProduct>(ProductType.AromaBombel)
            .HasValue<AromaBottleProduct>(ProductType.AromaBottle)
            .HasValue<AromaDeviceProduct>(ProductType.AromaDevice)
            .HasValue<SanitizingDeviceProduct>(ProductType.SanitizingDevice)
            .HasValue<BatteryProduct>(ProductType.Battery);

        // Configure ProductName value object
        builder.Property(p => p.Name)
            .HasConversion(
                name => name.Value,
                value => ProductName.Create(value))
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        // Configure Description
        builder.Property(p => p.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        // Configure Price value object (Money) - Split into two columns
        builder.OwnsOne(p => p.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount)
                .HasColumnName("Price")
                .HasPrecision(18, 2)
                .IsRequired();

            priceBuilder.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        // Configure PhotoUrl
        builder.Property(p => p.PhotoUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        // Configure StockQuantity
        builder.Property(p => p.StockQuantity)
            .IsRequired()
            .HasDefaultValue(0);

        // Configure IsActive
        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Configure timestamps
        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        // Ignore constructor for materialization - use parameterless constructor
        builder.Metadata.SetNavigationAccessMode(PropertyAccessMode.Field);

        // Indexes
        // Unique composite index on Name and ProductType - ensures name is unique within each category
        builder.HasIndex(p => new { p.Name, p.ProductType })
            .IsUnique()
            .HasDatabaseName("IX_Products_Name_ProductType");

        // Index on ProductType for filtering by category
        builder.HasIndex(p => p.ProductType)
            .HasDatabaseName("IX_Products_ProductType");

        // Index on IsActive for filtering active/inactive products
        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Products_IsActive");
    }
}
