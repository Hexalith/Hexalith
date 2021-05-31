namespace Hexalith.Application.Queries
{
    using System;

    public interface IQueryBusBuilder
    {
        IQueryBusBuilder AddQueryHandler<TQuery, TResult>(Func<IQueryHandler<TQuery, TResult>> handler)
            where TQuery : class, IQuery<TResult>;

        IQueryBus Build();
    }
}