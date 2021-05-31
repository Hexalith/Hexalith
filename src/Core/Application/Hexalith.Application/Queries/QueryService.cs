namespace Hexalith.Application.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;
    using Hexalith.Application.Services;
    using Hexalith.Domain.ValueTypes;

    public class QueryService : IQueryService
    {
        private readonly IQueryBus _queryBus;
        private readonly IUserIdentity _userIdentity;

        public QueryService(IQueryBus queryBus, IUserIdentity userIdentity)
        {
            _queryBus = queryBus;
            _userIdentity = userIdentity;
        }

        public Task<TResult> Ask<TQuery, TResult>(TQuery query, string? messageId = null, CancellationToken cancellationToken = default)
            where TQuery : class
            => _queryBus.Dispatch<TQuery, TResult>(new Envelope<TQuery>(query,
                                                                        messageId ?? new MessageId(),
                                                                        _userIdentity.UserName,
                                                                        DateTimeOffset.Now), cancellationToken);
    }
}