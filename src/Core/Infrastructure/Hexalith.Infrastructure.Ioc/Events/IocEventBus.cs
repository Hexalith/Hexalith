namespace Hexalith.Infrastructure.Ioc.Events
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Events;
    using Hexalith.Application.Exceptions;
    using Hexalith.Application.Messages;

    public class IocEventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        public IocEventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task Publish<TEvent>(Envelope<TEvent> envelope, CancellationToken cancellationToken = default)
            where TEvent : class
        {
            Type handlerType = MakeEventHandlerInterface(typeof(TEvent));
            IEnumerable services = _serviceProvider
                .GetService(MakeEventHandlerCollection(handlerType)) as IEnumerable
                    ?? throw new InvalidEventHandlerTypeException($"Cannot retreive a collection of '{handlerType.FullName}' ");
            foreach (var service in services)
            {
                await ((IEventHandler<TEvent>)service).Handle(envelope, cancellationToken);
            }
        }

        public async Task Publish(IEnvelope envelope, CancellationToken cancellationToken = default)
        {
            Type handlerType = MakeEventHandlerInterface(envelope.Message.GetType());
            IEnumerable services = _serviceProvider
                .GetService(MakeEventHandlerCollection(handlerType)) as IEnumerable
                    ?? throw new InvalidEventHandlerTypeException($"Cannot retreive a collection of '{handlerType.FullName}' ");
            foreach (var handler in services)
            {
                var handleMethod = handler.GetType().GetMethod("Handle", new[] { typeof(IEnvelope), typeof(CancellationToken) });
                if (handleMethod == null)
                {
                    throw new InvalidEventHandlerTypeException($"Handle method not found on handler '{handler.GetType().FullName}'.");
                }
                if (handleMethod.Invoke(handler, new object[] { envelope, cancellationToken }) is Task resultTask)
                {
                    await resultTask.ConfigureAwait(false);
                }
                else
                {
                    throw new InvalidEventHandlerTypeException($"Handle method returns null on handler '{handler.GetType().FullName}'.");
                }
            }
        }

        private static Type MakeEventHandlerCollection(Type handlerType)
        {
            Type collectionType = typeof(IEnumerable<>).MakeGenericType(new Type[] { handlerType });
            if (collectionType == null)
            {
                throw new InvalidEventHandlerTypeException($"Cannot create the type IEnumerable<{handlerType.Name}>.");
            }
            return collectionType;
        }

        private static Type MakeEventHandlerInterface(Type EventType)
        {
            Type handlerType = typeof(IEventHandler<>).MakeGenericType(new Type[] { EventType });
            if (handlerType == null)
            {
                throw new InvalidEventHandlerTypeException($"Cannot create the type IEventHandler<{EventType.Name}>.");
            }
            return handlerType;
        }
    }
}