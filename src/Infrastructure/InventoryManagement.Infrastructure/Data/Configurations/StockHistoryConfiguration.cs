using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the StockHistory entity.
/// Configures the audit trail table for all stock movements.
/// Note: No foreign key to Product to preserve history even if product is deleted.
/// </summary>
public class StockHistoryConfiguration : IEntityTypeConfiguration<StockHistory>
{
    public void Configure(EntityTypeBuilder<StockHistory> builder)
    {
        // Table name
        builder.ToTable("StockHistories");

        // Primary key
        builder.HasKey(sh => sh.Id);

        // Configure ProductId (no foreign key constraint)
        builder.Property(sh => sh.ProductId)
            .IsRequired();

        // Configure QuantityChanged
        builder.Property(sh => sh.QuantityChanged)
            .IsRequired();

        // Configure QuantityAfter
        builder.Property(sh => sh.QuantityAfter)
            .IsRequired();

        // Configure ChangeType
        builder.Property(sh => sh.ChangeType)
            .HasMaxLength(20)
            .IsRequired();

        // Configure Reason
        builder.Property(sh => sh.Reason)
            .HasMaxLength(500)
            .IsRequired(false);

        // Configure ChangedBy (User ID)
        builder.Property(sh => sh.ChangedBy)
            .IsRequired();

        // Configure ChangedAt
        builder.Property(sh => sh.ChangedAt)
            .IsRequired();

        // Indexes
        // Index on ProductId for querying stock history by product
        builder.HasIndex(sh => sh.ProductId)
            .HasDatabaseName("IX_StockHistories_ProductId");

        // Index on ChangedAt (descending) for date-based queries and sorting
        builder.HasIndex(sh => sh.ChangedAt)
            .IsDescending()
            .HasDatabaseName("IX_StockHistories_ChangedAt");

        // Index on ChangedBy for querying by user
        builder.HasIndex(sh => sh.ChangedBy)
            .HasDatabaseName("IX_StockHistories_ChangedBy");
    }
}
