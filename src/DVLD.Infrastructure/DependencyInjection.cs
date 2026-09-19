using System.Reflection;
using DVLD.Application.Interfaces.Events;
using DVLD.Application.Interfaces.Persistence;
using DVLD.Infrastructure.Events;
using DVLD.Infrastructure.Persistence;
using DVLD.Infrastructure.Persistence.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DVLD.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure core services, database persistence components, 
    /// and dynamically discovers and registers all domain event handlers.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register core persistence and event orchestration infrastructure
        services.AddScoped<DbSession>();
        services.AddScoped<IEventAggregator, EventAggregator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Scan and register open-generic event handlers across the application assembly
        services.RegisterDomainEventHandlers();

        return services;
    }

    /// <summary>
    /// Scans the application assembly using reflection to automatically register all implementations
    /// of <see cref="IBeforeCommitHandler{T}"/> and <see cref="IAfterCommitHandler{T}"/> with Scoped lifetime.
    /// </summary>
    private static void RegisterDomainEventHandlers(this IServiceCollection services)
    {
        // Locate the application assembly containing the domain event handlers
        var applicationAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "DVLD.Application")
            ?? Assembly.Load("DVLD.Application");

        var handlerInterfaceTypes = new[]
        {
            typeof(IBeforeCommitHandler<>),
            typeof(IAfterCommitHandler<>)
        };

        // Scan concrete classes and register matching generic handler interfaces
        foreach (var type in applicationAssembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            var implementedInterfaces = type.GetInterfaces();

            foreach (var implementedInterface in implementedInterfaces)
            {
                if (implementedInterface.IsGenericType &&
                    handlerInterfaceTypes.Contains(implementedInterface.GetGenericTypeDefinition()))
                {
                    // Register the closed generic interface with its concrete implementation
                    // Example: services.AddScoped<IBeforeCommitHandler<LicenseSuspendedEvent>, SuspendLicenseHandler>();
                    services.AddScoped(implementedInterface, type);
                }
            }
        }
    }
}