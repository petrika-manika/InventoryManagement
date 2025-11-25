using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Application.Common.Interfaces;

/// <summary>
/// Database context interface following the Dependency Inversion Principle.
/// The Application layer defines what it needs, and the Infrastructure layer implements it.
/// This allows the Application layer to remain independent of the database implementation.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Gets the Users DbSet for accessing the users table.
    /// </summary>
    DbSet<User> Users { get; }

    /// <summary>
    /// Gets the Products DbSet for accessing all products.
    /// Uses Table Per Hierarchy (TPH) pattern with discriminator column.
    /// </summary>
    DbSet<Product> Products { get; }

    /// <summary>
    /// Gets the StockHistories DbSet for accessing stock movement audit trail.
    /// </summary>
    DbSet<StockHistory> StockHistories { get; }

    /// <summary>
    /// Gets the Clients DbSet for accessing all clients (Individual and Business).
    /// Uses Table Per Hierarchy (TPH) pattern with discriminator column.
    /// </summary>
    DbSet<Client> Clients { get; }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
