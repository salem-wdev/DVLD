using DVLD.Domain.Common;

namespace DVLD.Application.Interfaces.Events;

public interface IBeforeCommitHandler<in TEvent> : IEventHandler
    where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event);
}