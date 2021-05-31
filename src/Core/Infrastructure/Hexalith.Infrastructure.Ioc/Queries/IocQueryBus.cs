namespace Hexalith.Infrastructure.Ioc.Queries
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Exceptions;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;

    public class IocQueryBus : IQueryBus
    {
        private readonly IServiceProvider _serviceProvider;

        public IocQueryBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Task<TResult> Dispatch<TQuery, TResult>(Envelope<TQuery> envelope, CancellationToken cancellationToken = default)
            where TQuery : class
        {
            Type handlerType = MakeQueryHandlerInterface(typeof(TQuery), typeof(TResult));
            object? service = _serviceProvider.GetService(handlerType);
            if (service == null)
            {
                return Task.FromException<TResult>(new QueryHandlerNotFoundException(envelope.Message.GetType()));
            }
            IQueryHandler<TQuery, TResult>? handler = service as IQueryHandler<TQuery, TResult>;
            return handler?.Handle(envelope, cancellationToken)
                ?? Task.FromException<TResult>(new InvalidQueryHandlerTypeException(service.GetType(), typeof(IQueryHandler<TQuery, TResult>)));
        }

        public async Task<object?> Dispatch(IEnvelope envelope, CancellationToken cancellationToken = default)
        {
            Type? genericQueryType = Array.Find(envelope.Message.GetType().GetInterfaces(), p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IQuery<>));
            if (genericQueryType == null)
            {
                throw new InvalidQueryTypeException(envelope.Message.GetType(), $"Does not implement type {typeof(IQuery<>).Name}");
            }
            Type? resultType = genericQueryType.GenericTypeArguments.FirstOrDefault();
            if (resultType == null)
            {
                throw new InvalidQueryTypeException(envelope.Message.GetType(), $"Can't retreive generic type {typeof(IQuery<>).Name} argument.");
            }

            Type handlerType = MakeQueryHandlerInterface(envelope.Message.GetType(), resultType);
            object? service = _serviceProvider.GetService(handlerType);
            if (service == null)
            {
                throw new QueryHandlerNotFoundException(envelope.Message.GetType());
            }
            var handleMethod = service.GetType().GetMethod("Handle", new[] { envelope.GetType() });
            if (handleMethod == null)
            {
                throw new InvalidQueryHandlerTypeException($"Handle method not found on handler '{service.GetType().FullName}'.");
            }
            if (handleMethod.Invoke(service, new object[] { envelope, cancellationToken }) is Task<object> resultTask)
            {
                var result = await resultTask.ConfigureAwait(false);
                if (resultType.IsInstanceOfType(result))
                {
                    return result;
                }
                throw new InvalidQueryHandlerTypeException($"Handle method returns a value with an invalid type on handler '{service.GetType().FullName}': Expected:{resultType.Name}; Result:{result.GetType().Name}.");
            }
            else
            {
                throw new InvalidQueryHandlerTypeException($"Handle method returns null on handler '{service.GetType().FullName}'.");
            }
        }

        private static Type MakeQueryHandlerInterface(Type queryType, Type resultType)
        {
            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(new Type[] { queryType, resultType });
            if (handlerType == null)
            {
                throw new InvalidQueryHandlerTypeException($"Cannot create the type IQueryHandler<{queryType.Name},{resultType.Name}>.");
            }
            return handlerType;
        }
    }
}