using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InventoryManagement.Infrastructure.Data;

/// <summary>
/// Entity Framework Core DbContext implementing IApplicationDbContext.
/// Provides database access for the application following the Repository pattern.
/// Configures entity mappings using IEntityTypeConfiguration implementations.
/// </summary>
public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The DbContext options.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the Users DbSet for accessing user data in the database.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Gets the Products DbSet for accessing all products in the database.
    /// Uses Table Per Hierarchy (TPH) pattern with discriminator column.
    /// </summary>
    public DbSet<Product> Products => Set<Product>();

    /// <summary>
    /// Gets the StockHistories DbSet for accessing stock movement audit trail.
    /// </summary>
    public DbSet<StockHistory> StockHistories => Set<StockHistory>();

    /// <summary>
    /// Gets the Clients DbSet for accessing all clients in the database.
    /// Uses Table Per Hierarchy (TPH) pattern with discriminator column.
    /// </summary>
    public DbSet<Client> Clients => Set<Client>();

    /// <summary>
    /// Configures the entity models using Fluent API.
    /// Automatically applies all IEntityTypeConfiguration implementations from the current assembly.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure the entity models.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
