namespace Hexalith.Infrastructure.Ioc.Commands
{
    using Hexalith.Application.Commands;
    using Hexalith.Application.Exceptions;
    using Hexalith.Application.Messages;

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class IocCommandBus : ICommandBus
    {
        private readonly IServiceProvider _serviceProvider;

        public IocCommandBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Task Send<TCommand>(Envelope<TCommand> envelope, CancellationToken cancellationToken = default)
            where TCommand : class
        {
            Type handlerType = MakeCommandHandlerInterface(typeof(TCommand));
            object? service = _serviceProvider.GetService(handlerType);
            if (service == null)
            {
                return Task.FromException(new CommandHandlerNotFoundException(envelope.Message.GetType()));
            }
            ICommandHandler<TCommand>? handler = service as ICommandHandler<TCommand>;
            return handler?.Handle(envelope, cancellationToken)
                ?? Task.FromException(new InvalidCommandHandlerTypeException(service.GetType(), typeof(ICommandHandler<TCommand>)));
        }

        public async Task Send(IEnvelope envelope, CancellationToken cancellationToken = default)
        {
            Type handlerType = MakeCommandHandlerInterface(envelope.Message.GetType());
            object? service = _serviceProvider.GetService(handlerType);
            if (service == null)
            {
                throw new CommandHandlerNotFoundException(envelope.Message.GetType());
            }
            var handleMethod = service.GetType().GetMethod("Handle", new[] { envelope.GetType(), typeof(CancellationToken) });
            if (handleMethod == null)
            {
                throw new InvalidCommandHandlerTypeException($"Handle method not found on handler '{service.GetType().FullName}'.");
            }
            if (handleMethod.Invoke(service, new object[] { envelope, cancellationToken }) is Task resultTask)
            {
                await resultTask.ConfigureAwait(false);
            }
            else
            {
                throw new InvalidCommandHandlerTypeException($"Handle method returns null on handler '{service.GetType().FullName}'.");
            }
        }

        private static Type MakeCommandHandlerInterface(Type CommandType)
        {
            Type handlerType = typeof(ICommandHandler<>).MakeGenericType(new Type[] { CommandType });
            if (handlerType == null)
            {
                throw new InvalidCommandHandlerTypeException($"Cannot create the type ICommandHandler<{CommandType.Name}>.");
            }
            return handlerType;
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