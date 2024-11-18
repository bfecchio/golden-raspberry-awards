using MediatR;

namespace Awards.Domain.Core.Events
{
    public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    { }
}
