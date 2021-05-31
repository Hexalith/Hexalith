namespace Hexalith.Application.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;

    public interface IQueryHandler
    {
        public bool CanHandle(Type queryType);

        Task<object?> Handle(IEnvelope envelope, CancellationToken cancellationToken = default);
    }

    public interface IQueryHandler<TQuery, TResult> : IQueryHandler
        where TQuery : class
    {
        Task<TResult> Handle(Envelope<TQuery> envelope, CancellationToken cancellationToken = default);
    }
}