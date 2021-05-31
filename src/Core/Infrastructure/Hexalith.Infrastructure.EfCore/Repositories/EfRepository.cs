namespace Hexalith.Infrastructure.EfCore.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Events;
    using Hexalith.Application.Exceptions;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.Domain.ValueTypes;
    using Hexalith.Infrastructure.EfCore.Helpers;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class EfRepository<TIState, TState> : RepositoryBase<TIState>
        where TState : TIState, new()
    {
        private readonly StateStoreDbContext _context;
        private readonly IEventBus _eventBus;
        private readonly ILogger<EfRepository<TIState, TState>> _logger;
        private readonly string _sessionId = new AutoIdentifier();

        public EfRepository(StateStoreDbContext context, IEventBus eventBus, ILogger<EfRepository<TIState, TState>> logger)
        {
            _context = context;
            _eventBus = eventBus;
            _logger = logger;
        }

        public override async Task AddStateLog(string id, IRepositoryMetadata metadata, IEnumerable<object> events, CancellationToken cancellationToken = default)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }
            int version = await GetStreamLatestVersion(id, cancellationToken).ConfigureAwait(false);
            _context.Add(GetStateStreamItem(events, id, metadata, version + 1));
        }

        public override async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
            => await _context
                .States
                .FindAsync(GetKey(id), cancellationToken)
                .ConfigureAwait(false) != null;

        public override async Task<IRepositoryStateMetadata> GetMetadata(string id, CancellationToken cancellationToken = default)
            => await GetRecord(id, cancellationToken)
                .ConfigureAwait(false);

        public override async Task<TIState> GetState(string id, CancellationToken cancellationToken = default)
        {
            var value = (await GetRecord(id, cancellationToken)
                            .ConfigureAwait(false)
                        ).Value;
            return value is TIState state ? state : throw new RepositoryStateDeserializeException(this, id);
        }

        public override Task<IRepositoryStream> GetStream(string id, CancellationToken cancellationToken = default)
            => Task.FromException<IRepositoryStream>(new NotSupportedException($"The repository {GetType().Name} does not support event streams. Trying to retreive stream '{id}'."));

        public override Task Publish(IEnumerable<IEnvelope> events, CancellationToken cancellationToken = default)
        {
            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }
            foreach (var envelope in events)
            {
                _context.Add(envelope.ToOutboxMessage(_sessionId));
            }
            return Task.CompletedTask;
        }

        public Task Save(string id, IRepositoryData stateData, CancellationToken cancellationToken = default)
                    => Save(id, new RepositoryData<TState>(stateData), cancellationToken);

        public override async Task Save(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await PublishOutboxMessages(cancellationToken).ConfigureAwait(false);
        }

        public override async Task SetState(string id, IRepositoryMetadata metadata, TIState state, CancellationToken cancellationToken = default)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }
            if (state == null)
            {
                throw new RepositoryStateNullException(
                        this,
                        id, $"The state object given for saving Id='{id}' is null. Repository:'{GetType().Name}'.");
            }
            State? data = await _context.States
                .FindAsync(GetKey(id), cancellationToken)
                .ConfigureAwait(false);
            if (data == null)
            {
                data = (State?)(new() { Id = GetStateId(id), CreatedByUser = metadata.UserName, CreatedUtcDateTime = DateTime.UtcNow });
                _context.States.Add(data);
            }
            else
            {
                data.LastModifiedByUser = metadata.UserName;
                data.LastModifiedUtcDateTime = DateTime.UtcNow;
            }
            data.Value = state;
        }

        private static object[] GetKey(string id)
        {
            var key = GetStateId(id);
            return new object[] { key.HashKey(), key };
        }

        private static string GetStateId(string id)
            => typeof(TIState).Name + "@" + id;

        private static StateStreamItem GetStateStreamItem(IEnumerable<object> events, string id, IRepositoryMetadata metadata, int version)
        {
            var stateId = GetStateId(id);
            return new()
            {
                CausationId = metadata.CausationId,
                CorrelationId = metadata.CorrelationId,
                Events = JsonSerializer.Serialize(events),
                Id = stateId,
                IdHash = stateId.HashKey(),
                UserName = metadata.UserName,
                SystemUtcDateTime = DateTime.UtcNow,
                Version = version
            };
        }

        private async Task<State> GetRecord(string id, CancellationToken cancellationToken)
        {
            State? state = await _context
                .States
                .FindAsync(GetKey(id), cancellationToken)
                .ConfigureAwait(false);
            return state ?? throw new RepositoryStateNotFoundException(this, id);
        }

        private Task<int> GetStreamLatestVersion(string id, CancellationToken cancellationToken = default)
            => _context
            .StateStreams
            .Where(p => p.IdHash == id.HashKey() && p.Id == id)
            .OrderByDescending(p => p.Version)
            .Select(p => p.Version)
            .FirstOrDefaultAsync(cancellationToken);

        private async Task PublishOutboxMessage(int id, CancellationToken cancellationToken = default)
        {
            OutboxMessage outboxMessage = await _context.MessageOutbox.FindAsync(new object[] { id }, cancellationToken).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"{nameof(OutboxMessage)} with Id={id} not found.");
            outboxMessage.InProgressSince = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            Type? messageType = Type.GetType(outboxMessage.EventType);
            if (messageType == null)
            {
                throw new TypeInitializationException(outboxMessage.EventType, null);
            }
            var message = JsonSerializer.Deserialize(outboxMessage.Event, messageType);
            if (message == null)
            {
                throw new RepositoryOutboxMessageDeserializeException(this, outboxMessage.MessageId, string.Empty);
            }
            MessageId? correlationId = (outboxMessage.CorrelationId == null) ? null : new(outboxMessage.CorrelationId);
            MessageId? causationId = (outboxMessage.CausationId == null) ? null : new(outboxMessage.CausationId);
            try
            {
                await _eventBus.Publish(new Envelope(
                    message,
                    outboxMessage.MessageId,
                    outboxMessage.UserName,
                    outboxMessage.UserDateTime,
                    correlationId,
                    causationId)
                , cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                outboxMessage.SentUtcDateTime = null;
                outboxMessage.InProgressSince = null;
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            outboxMessage.SentUtcDateTime = DateTime.UtcNow;
            outboxMessage.InProgressSince = null;
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task PublishOutboxMessages(CancellationToken cancellationToken = default)
        {
            foreach (var id in await _context
                    .MessageOutbox
                    .Where(p => p.SentUtcDateTime == null && p.SessionId == _sessionId)
                    .OrderBy(p => p.Id)
                    .Select(p => p.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false))
            {
                await PublishOutboxMessage(id, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}