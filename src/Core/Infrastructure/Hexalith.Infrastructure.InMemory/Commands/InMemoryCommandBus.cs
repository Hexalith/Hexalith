namespace Hexalith.Infrastructure.InMemory.Commands
{
    using Hexalith.Application.Commands;
    using Hexalith.Application.Exceptions;
    using Hexalith.Application.Messages;

    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Implements the <see cref="ICommandBus"/>.
    /// </summary>
    /// <seealso cref="ICommandBus"/>
    /// <autogeneratedoc/>
    public class InMemoryCommandBus : ICommandBus
    {
        private readonly ImmutableDictionary<Type, Func<ICommandHandler>> _handlers;

        public InMemoryCommandBus(Dictionary<Type, Func<ICommandHandler>> handlers)
        {
            _handlers = handlers?.ToImmutableDictionary() ?? throw new ArgumentNullException(nameof(handlers));
        }

        public Task Send<TCommand>(Envelope<TCommand> envelope, CancellationToken cancellationToken = default)
            where TCommand : class
        {
            if (!_handlers.TryGetValue(envelope.Message.GetType(), out Func<ICommandHandler>? handlerFunc))
            {
                return Task.FromException(new CommandHandlerNotFoundException(envelope.Message.GetType()));
            }
            ICommandHandler<TCommand>? handler = handlerFunc() as ICommandHandler<TCommand>;
            return handler?.Handle(envelope, cancellationToken)
                ?? Task.FromException(new InvalidCommandHandlerTypeException(handlerFunc().GetType(), typeof(ICommandHandler<TCommand>)));
        }

        public Task Send(IEnvelope envelope, CancellationToken cancellationToken = default)
        {
            if (!_handlers.TryGetValue(envelope.Message.GetType(), out Func<ICommandHandler>? handlerFunc))
            {
                return Task.FromException<object?>(new CommandHandlerNotFoundException(envelope.Message.GetType()));
            }
            ICommandHandler? handler = handlerFunc();
            return handler?.Handle(envelope, cancellationToken)
                ?? Task.FromException<object?>(new InvalidCommandHandlerTypeException(handlerFunc().GetType(), typeof(ICommandHandler)));
        }

        public async Task Send(IEnumerable<IEnvelope> list, CancellationToken cancellationToken = default)
        {
            foreach (var e in list)
            {
                await Send(e, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}