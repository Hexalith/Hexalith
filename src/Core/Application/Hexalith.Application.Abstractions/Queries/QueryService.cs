using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Messages;
using Hexalith.Domain.ValueTypes;

namespace Hexalith.Application.Queries
{
    public class DispatchQueryService : IQueryService
    {
        private readonly IQueryBus _queryDispatcher;
        private readonly IPrincipal _user;

        public DispatchQueryService(IQueryBus queryDispatcher, IPrincipal user)
        {
            _queryDispatcher = queryDispatcher;
            _user = user;
        }

        public Task<TResult> Ask<TQuery, TResult>(TQuery query, string? messageId = null, CancellationToken cancellationToken = default) where TQuery : class
           => _queryDispatcher
                   .Dispatch<TQuery, TResult>(new Envelope<TQuery>(query, messageId ?? new MessageId(), _user?.Identity?.Name, DateTimeOffset.Now), cancellationToken);
    }
}