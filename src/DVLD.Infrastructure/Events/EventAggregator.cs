using DVLD.Application.Interfaces.Events;
using DVLD.Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace DVLD.Infrastructure.Events;

/// <summary>
/// Dispatches domain events to their corresponding in-transaction and post-commit handlers
/// dynamically via the dependency injection container.
/// </summary>
public sealed class EventAggregator : IEventAggregator
{
    private readonly IServiceProvider _serviceProvider;

    public EventAggregator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Dispatches domain events to handlers participating within the active database transaction scope.
    /// </summary>
    public async Task PublishBeforeCommitAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            // Resolve the closed generic handler type at runtime (e.g., IBeforeCommitHandler<LicenseSuspendedEvent>)
            var handlerType = typeof(IBeforeCommitHandler<>).MakeGenericType(domainEvent.GetType());

            // Retrieve all registered handlers for the resolved type from the DI container
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler is null)
                {
                    continue;
                }

                // Locate the contract method defined by the interface
                var method = handlerType.GetMethod(nameof(IBeforeCommitHandler<IDomainEvent>.HandleAsync));
                if (method is not null)
                {
                    // Invoke the handler asynchronously and pass the event payload
                    var task = (Task?)method.Invoke(handler, new object[] { domainEvent });
                    if (task is not null)
                    {
                        await task;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Dispatches domain events to external side-effect handlers (e.g., notifications, logging)
    /// only after the database transaction has successfully committed.
    /// </summary>
    public async Task PublishAfterCommitAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            // Resolve the closed generic handler type for post-commit actions
            var handlerType = typeof(IAfterCommitHandler<>).MakeGenericType(domainEvent.GetType());

            // Retrieve all registered post-commit handlers from the DI container
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler is null)
                {
                    continue;
                }

                // Locate the execution method
                var method = handlerType.GetMethod(nameof(IAfterCommitHandler<IDomainEvent>.HandleAsync));
                if (method is not null)
                {
                    // Execute the side effect asynchronously
                    var task = (Task?)method.Invoke(handler, new object[] { domainEvent });
                    if (task is not null)
                    {
                        await task;
                    }
                }
            }
        }
    }
}