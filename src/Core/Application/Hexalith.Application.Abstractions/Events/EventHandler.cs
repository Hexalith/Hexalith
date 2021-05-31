#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace Hexalith.Application.Events
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;

    public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : class
    {
        public bool CanHandle(Type EventType)
        {
            return EventType == typeof(TEvent);
        }

        public abstract Task Handle(Envelope<TEvent> envelope, CancellationToken cancellationToken = default);

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
           => Handle(new Envelope<TEvent>(envelope), cancellationToken);
    }
}