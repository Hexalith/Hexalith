namespace Hexalith.DataIntegrations.Application.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Commands;
    using Hexalith.Application.Exceptions;
    using Hexalith.Application.Helpers;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.DataIntegrations.Domain;
    using Hexalith.DataIntegrations.Domain.States;
    using Hexalith.Domain.ValueTypes;

    using Microsoft.Extensions.Logging;

    [CommandHandler(Command = typeof(SubmitDataIntegration))]
    public class SubmitDataIntegrationHandler : ICommandHandler<SubmitDataIntegration>
    {
        private readonly ILogger<NormalizeDataIntegrationHandler> _logger;
        private readonly IRepository<IDataIntegrationState> _repository;

        public SubmitDataIntegrationHandler(IRepository<IDataIntegrationState> repository, ILogger<NormalizeDataIntegrationHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(Envelope<SubmitDataIntegration> envelope, CancellationToken cancellationToken = default)
        {
            try
            {
                var id = envelope.Message.DataIntegrationId;
                var state = new DataIntegrationState();
                var integration = new DataIntegration(id, state);
                var command = envelope.Message;
                var events = await integration.Submit(
                            name: envelope.Message.Name,
                            description: envelope.Message.Description,
                            documentName: envelope.Message.DocumentName,
                            documentType: envelope.Message.DocumentType,
                            document: envelope.Message.Document);

                await _repository.SetState(id, envelope.ToMetadata(), state, cancellationToken);
                await _repository.Publish(events.Select(p => new Envelope(p, new MessageId(), envelope)).ToList(), cancellationToken);
                await _repository.Save(cancellationToken);
            }
            catch (DuplicateRepositoryStateException)
            {
                _logger.LogWarning($"Duplicate integration submission : Id='{envelope.Message.DataIntegrationId}', Name='{envelope.Message.Name}', MessageId='{envelope.MessageId}'.");
            }
        }

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
            => Handle(new Envelope<SubmitDataIntegration>(envelope), cancellationToken);
    }
}