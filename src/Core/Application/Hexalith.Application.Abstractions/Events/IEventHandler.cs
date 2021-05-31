#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace Hexalith.Application.Events
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;

    public interface IEventHandler<TEvent> : IEventHandler
        where TEvent : class
    {
        Task Handle(Envelope<TEvent> envelope, CancellationToken cancellationToken = default);
    }

    public interface IEventHandler
    {
        Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default);
    }
}