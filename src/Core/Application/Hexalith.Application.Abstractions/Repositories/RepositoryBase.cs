namespace Hexalith.Application.Repositories
{
    using Hexalith.Application.Exceptions;
    using Hexalith.Application.Messages;

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class RepositoryBase<TState> : IRepository<TState>
    {
        public abstract Task<bool> Exists(string id, CancellationToken cancellationToken = default);

        public abstract Task<IRepositoryStateMetadata> GetMetadata(string id, CancellationToken cancellationToken = default);

        public abstract Task<TState> GetState(string id, CancellationToken cancellationToken = default);

        public abstract Task<IRepositoryStream> GetStream(string id, CancellationToken cancellationToken = default);

        public abstract Task Save(CancellationToken cancellationToken = default);

        async Task<object> IRepository.GetState(Type dataType, string id, CancellationToken cancellationToken)
                    => await GetState(id, cancellationToken).ConfigureAwait(false) ??
                throw new RepositoryStateNullException(this, id, GetType().Name);

        public abstract Task SetState(string id, IRepositoryMetadata metadata, TState state, CancellationToken cancellationToken = default);

        public abstract Task AddStateLog(string id, IRepositoryMetadata metadata, IEnumerable<object> events, CancellationToken cancellationToken = default);

        public abstract Task Publish(IEnumerable<IEnvelope> events, CancellationToken cancellationToken = default);

        Task IRepository.SetState(string id, IRepositoryMetadata metadata, object state, CancellationToken cancellationToken)
            => SetState(id, metadata, (TState)state, cancellationToken);

    }
}