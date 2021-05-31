namespace Hexalith.Infrastructure.InMemory.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Events;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;

    public class InMemoryRepository<TIState, TState> : RepositoryBase<TIState>
        where TState : TIState, new()
    {
        private readonly IEventBus _eventBus;
        private readonly List<IEnvelope> _outbox = new();
        private readonly Dictionary<string, (TIState, IRepositoryStateMetadata)> _states = new();
        private readonly Dictionary<string, List<(IEnumerable<object>, IRepositoryStateMetadata)>> _streams = new();

        public InMemoryRepository(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public override Task AddStateLog(string id, IRepositoryMetadata metadata, IEnumerable<object> events, CancellationToken cancellationToken = default)
        {
            if (!_streams.ContainsKey(id))
            {
                _streams[id] = new()
                {
                    (events, new RepositoryStateMetadata()
                    {
                        CreatedByUser = metadata.UserName,
                        CreatedUtcDateTime = metadata.SystemUtcDateTime
                    })
                };
            }
            else
            {
                (_, IRepositoryStateMetadata m) = GetById(id);
                m.LastModifiedByUser = metadata.UserName;
                m.LastModifiedUtcDateTime = metadata.SystemUtcDateTime;
                _streams[id].Add((events, m));
            }
            return Task.CompletedTask;
        }

        public override Task<bool> Exists(string id, CancellationToken cancellationToken = default) => Task.FromResult(_states.ContainsKey(id));

        public override Task<IRepositoryStateMetadata> GetMetadata(string id, CancellationToken cancellationToken = default)
        {
            (_, IRepositoryStateMetadata metadata) = GetById(id);
            return Task.FromResult(metadata);
        }

        public override Task<TIState> GetState(string id, CancellationToken cancellationToken = default)
        {
            (TIState state, _) = GetById(id);
            return Task.FromResult(state);
        }

        public override Task<IRepositoryStream> GetStream(string id, CancellationToken cancellationToken = default)
            => Task.FromException<IRepositoryStream>(new NotSupportedException($"The '{GetType().Name}' repository does not support streams."));

        public override Task Publish(IEnumerable<IEnvelope> events, CancellationToken cancellationToken = default)
        {
            foreach (var envelope in events)
            {
                _outbox.Add(envelope);
            }
            return Task.CompletedTask;
        }

        public override async Task Save(CancellationToken cancellationToken = default)
        {
            foreach (var envelope in _outbox)
            {
                await _eventBus.Publish(envelope, cancellationToken).ConfigureAwait(false);
                _outbox.Remove(envelope);
            }
        }

        public override Task SetState(string id, IRepositoryMetadata metadata, TIState state, CancellationToken cancellationToken = default)
        {
            if (!_states.ContainsKey(id))
            {
                _states[id] = (state, new RepositoryStateMetadata()
                {
                    CreatedByUser = metadata.UserName,
                    CreatedUtcDateTime = metadata.SystemUtcDateTime
                });
            }
            else
            {
                (_, IRepositoryStateMetadata m) = GetById(id);
                m.LastModifiedByUser = metadata.UserName;
                m.LastModifiedUtcDateTime = metadata.SystemUtcDateTime;
                _states[id] = (state, m);
            }
            return Task.CompletedTask;
        }

        private (TIState state, IRepositoryStateMetadata metadata) GetById(string id)
        {
            if (!_states.TryGetValue(id, out (TIState, IRepositoryStateMetadata) data))
            {
                throw new KeyNotFoundException($"The storage object (Type='{typeof(TIState)}';Id='{id}') not found.");
            }
            return data;
        }
    }
}