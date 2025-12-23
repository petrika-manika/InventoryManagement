using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Respawn;

namespace InventoryManagement.API.Tests;

/// <summary>
/// Custom WebApplicationFactory for integration tests.
/// Configures a separate test database and provides database reset functionality using Respawn.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private Respawner _respawner = null!;
    private SqlConnection _connection = null!;
    private readonly string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=InventoryManagementDb_TEST;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true";

    /// <summary>
    /// Configures the web host for testing.
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add DbContext with test database
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_connectionString);
                options.EnableSensitiveDataLogging(); // Helpful for debugging tests
                options.EnableDetailedErrors();
            });

            // Ensure IApplicationDbContext is registered
            services.RemoveAll<IApplicationDbContext>();
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());
        });

        // Use test environment
        builder.UseEnvironment("Testing");

        // Suppress EF Core logging in tests
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddDebug();
            logging.SetMinimumLevel(LogLevel.Warning);
        });
    }

    /// <summary>
    /// Initializes the test database before any tests run.
    /// Creates the database and applies migrations.
    /// </summary>
    public async Task InitializeAsync()
    {
        // Ensure database is created
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        try
        {
            // Create database if it doesn't exist
            // This will use the current model to create the schema
            await context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to initialize test database. Make sure SQL Server LocalDB is running. Error: {ex.Message}", ex);
        }

        // Create a connection for Respawn
        _connection = new SqlConnection(_connectionString);
        await _connection.OpenAsync();

        // Initialize Respawn
        try
        {
            _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer,
                SchemasToInclude = new[] { "dbo" },
                TablesToIgnore = new[] 
                { 
                    new Respawn.Graph.Table("__EFMigrationsHistory") // Don't reset migration history
                },
                WithReseed = true // Reset identity columns
            });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to initialize Respawn. Error: {ex.Message}", ex);
        }

        // Seed initial data
        await ResetDatabaseAsync();
    }

    /// <summary>
    /// Resets the test database to a clean state.
    /// Call this before each test to ensure isolation.
    /// </summary>
    public async Task ResetDatabaseAsync()
    {
        try
        {
            await _respawner.ResetAsync(_connection);

            // Re-seed default admin user after reset
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        
            await ApplicationDbContextSeed.SeedDefaultUserAsync(context, passwordHasher);
        }
        catch (Exception ex)
        {
            // If seeding fails (e.g., user already exists), it's okay for tests
            // Tests can create their own users if needed
            Console.WriteLine($"Warning during database reset: {ex.Message}");
        }
    }

    /// <summary>
    /// Cleans up resources after all tests complete.
    /// </summary>
    public new async Task DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
        
        await base.DisposeAsync();
    }
}
