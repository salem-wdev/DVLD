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
    /// Registers database persistence, event dispatching infrastructure, 
    /// and automatically scans and registers domain event handlers.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register core lifecycle services
        services.AddScoped<DbSession>();
        services.AddScoped<IEventAggregator, EventAggregator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register handlers using the marker interface
        services.RegisterDomainEventHandlers();

        return services;
    }

    /// <summary>
    /// Scans the application assembly for concrete types implementing <see cref="IEventHandler"/>
    /// and registers their corresponding closed generic interfaces.
    /// </summary>
    private static void RegisterDomainEventHandlers(this IServiceCollection services)
    {
        var applicationAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "DVLD.Application")
            ?? Assembly.Load("DVLD.Application");

        var handlerTypes = applicationAssembly.GetTypes()
            .Where(t => typeof(IEventHandler).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

        foreach (var type in handlerTypes)
        {
            var genericInterfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType &&
                           (i.GetGenericTypeDefinition() == typeof(IBeforeCommitHandler<>) ||
                            i.GetGenericTypeDefinition() == typeof(IAfterCommitHandler<>)));

            foreach (var implementedInterface in genericInterfaces)
            {
                services.AddScoped(implementedInterface, type);
            }
        }
    }
}