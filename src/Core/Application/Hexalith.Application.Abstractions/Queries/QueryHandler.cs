using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Messages;

namespace Hexalith.Application.Queries
{
    public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class
    {
        public bool CanHandle(Type queryType)
        {
            return queryType == typeof(TQuery);
        }

        public abstract Task<TResult> Handle(Envelope<TQuery> envelope, CancellationToken cancellationToken = default);

        public async Task<object?> Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
        {
            return await Handle(new Envelope<TQuery>(envelope), cancellationToken)
                .ConfigureAwait(false);
        }
    }
}