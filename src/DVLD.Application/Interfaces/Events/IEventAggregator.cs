using DVLD.Domain.Common;

namespace DVLD.Application.Interfaces.Events;

public interface IEventAggregator
{
    Task PublishBeforeCommitAsync(IEnumerable<IDomainEvent> domainEvents);
    Task PublishAfterCommitAsync(IEnumerable<IDomainEvent> domainEvents);
}