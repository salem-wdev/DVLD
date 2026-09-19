using DVLD.Domain.Common;

namespace DVLD.Application.Interfaces.Events;

/// <summary>
/// Defines operations for publishing domain events across pre-commit and post-commit subscribers.
/// </summary>
public interface IEventAggregator
{
    /// <summary>
    /// Dispatches events to handlers operating within the current database transaction scope.
    /// </summary>
    /// <param name="domainEvents">The collection of domain events to publish.</param>
    Task PublishBeforeCommitAsync(IEnumerable<IDomainEvent> domainEvents);

    /// <summary>
    /// Dispatches events to post-transaction handlers after changes are committed to storage.
    /// </summary>
    /// <param name="domainEvents">The collection of domain events to publish.</param>
    Task PublishAfterCommitAsync(IEnumerable<IDomainEvent> domainEvents);
}