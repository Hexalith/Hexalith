namespace Hexalith.Application.Repositories
{
    using Hexalith.Application.Messages;

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRepository
    {
        Task<bool> Exists(string id, CancellationToken cancellationToken = default);

        Task<IRepositoryStateMetadata> GetMetadata(string id, CancellationToken cancellationToken = default);

        Task<object> GetState(Type dataType, string id, CancellationToken cancellationToken = default);

        Task<IRepositoryStream> GetStream(string id, CancellationToken cancellationToken = default);

        Task AddStateLog(string id, IRepositoryMetadata metadata, IEnumerable<object> events, CancellationToken cancellationToken = default);

        Task Publish(IEnumerable<IEnvelope> events, CancellationToken cancellationToken = default);

        Task SetState(string id, IRepositoryMetadata metadata, object state, CancellationToken cancellationToken = default);

        Task Save(CancellationToken cancellationToken = default);
    }

    public interface IRepository<TState> : IRepository
    {
        Task<TState> GetState(string id, CancellationToken cancellationToken = default);

        Task SetState(string id, IRepositoryMetadata metadata, TState state, CancellationToken cancellationToken = default);
    }
}