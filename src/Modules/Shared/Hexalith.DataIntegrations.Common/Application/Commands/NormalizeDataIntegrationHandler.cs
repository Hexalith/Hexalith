namespace Hexalith.DataIntegrations.Application.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Commands;
    using Hexalith.Application.Helpers;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.DataIntegrations.Contracts.Commands;
    using Hexalith.DataIntegrations.Domain;
    using Hexalith.DataIntegrations.Domain.States;

    using Microsoft.Extensions.Logging;

    [CommandHandler(Command = typeof(NormalizeDataIntegration))]
    public class NormalizeDataIntegrationHandler : ICommandHandler<NormalizeDataIntegration>
    {
        private readonly ILogger<NormalizeDataIntegrationHandler> _logger;
        private readonly IRepository<IDataIntegrationState> _repository;

        public NormalizeDataIntegrationHandler(IRepository<IDataIntegrationState> repository, ILogger<NormalizeDataIntegrationHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(Envelope<NormalizeDataIntegration> envelope, CancellationToken cancellationToken = default)
        {
            try
            {
                var id = envelope.Message.DataIntegrationId;
                var state = await _repository.GetState(id, cancellationToken);
                var integration = new DataIntegration(id, state);
                var command = envelope.Message;
                var events = await integration.Normalize();
                await _repository.SetState(id, envelope.ToMetadata(), state, cancellationToken);
                await _repository.Publish(events.Select(p => new Envelope(p, new Hexalith.Domain.ValueTypes.MessageId(), envelope)).ToList(), cancellationToken);
                await _repository.Save(cancellationToken);
            }
            catch (Exception)
            {
                _logger.LogError($"Data integration normalization error : Id='{envelope.Message.DataIntegrationId}', MessageId='{envelope.MessageId}'.");
                throw;
            }
        }

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
            => Handle(new Envelope<NormalizeDataIntegration>(envelope), cancellationToken);
    }
}