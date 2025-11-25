using System.Reflection;
using FluentValidation;
using InventoryManagement.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Application;

/// <summary>
/// Dependency Injection configuration for the Application layer.
/// Registers all application services following the Dependency Inversion Principle.
/// The Application layer defines what it needs (interfaces), and Infrastructure implements them.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Application layer services to the dependency injection container.
    /// Automatically registers all MediatR handlers, validators, and pipeline behaviors from this assembly.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Get the current assembly containing all handlers, validators, and behaviors
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR with all handlers and pipeline behaviors
        services.AddMediatR(cfg =>
        {
            // Register all IRequestHandler implementations from this assembly
            cfg.RegisterServicesFromAssembly(assembly);

            // Register ValidationBehavior to automatically validate all requests
            // This runs before any handler is invoked
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Register all FluentValidation validators from this assembly
        // Validators are automatically discovered and injected into ValidationBehavior
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
