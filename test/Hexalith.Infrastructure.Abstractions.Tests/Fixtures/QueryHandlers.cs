namespace Hexalith.Infrastructure.Tests.Fixture
{
    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class Query1 : TestQuery<int>
    {
    }

    public sealed class Query2 : TestQuery<string>
    {
    }

    public sealed class Query3 : TestQuery<Guid>
    {
    }

    public sealed class Query4 : TestQuery<string>
    {
    }

    public class QueryHandlerGuid : QueryHandler<Query3, Guid>
    {
        public override Task<Guid> Handle(Envelope<Query3> envelope, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new Guid("66CABB1C-18E3-4E26-AE0F-EA603D9F11FB"));
        }
    }

    public class QueryHandlerId : QueryHandler<Query4, string>
    {
        public override Task<string> Handle(Envelope<Query4> envelope, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(envelope.Message.MessageId);
        }
    }

    public class QueryHandlerInt : QueryHandler<Query1, int>
    {
        public override Task<int> Handle(Envelope<Query1> envelope, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(1);
        }
    }

    public class QueryHandlers : Dictionary<Type, Func<IQueryHandler>>
    {
        public QueryHandlers()
        {
            Add(typeof(Query1), () => new QueryHandlerInt());
            Add(typeof(Query2), () => new QueryHandlerString());
            Add(typeof(Query3), () => new QueryHandlerGuid());
            Add(typeof(Query4), () => new QueryHandlerId());
        }
    }

    public class QueryHandlerString : QueryHandler<Query2, string>
    {
        public override Task<string> Handle(Envelope<Query2> envelope, CancellationToken cancellationToken = default)
        {
            return Task.FromResult("2");
        }
    }

    public abstract class TestQuery<TResult> : QueryBase<TResult>
    {
    }
}