namespace Hexalith.MexicanDigitalInvoice.Application.Events
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Hexalith.Application.Events;
    using Hexalith.Application.Helpers;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.DataIntegrations.Contracts.Events;
    using Hexalith.MexicanDigitalInvoice.Aggregates;
    using Hexalith.MexicanDigitalInvoice.Domain;
    using Hexalith.MexicanDigitalInvoice.Domain.States;
    using Hexalith.MexicanDigitalInvoice.Entities;

    using Microsoft.Extensions.Logging;

    [EventHandler(Event = typeof(DataIntegrationSubmitted))]
    public class DataIntegrationSubmittedHandler : IEventHandler<DataIntegrationSubmitted>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<DataIntegrationSubmittedHandler> _logger;
        private readonly IRepository<IMexicanDigitalInvoiceState> _repository;

        public DataIntegrationSubmittedHandler(
            IRepository<IMexicanDigitalInvoiceState> repository,
            IEventBus eventBus,
            ILogger<DataIntegrationSubmittedHandler> logger)
        {
            _repository = repository;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task Handle(Envelope<DataIntegrationSubmitted> envelope, CancellationToken cancellationToken = default)
        {
            if (envelope.Message.DocumentType == "Xml")
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(envelope.Message.Document))
                    {
                        throw new MexicanDigitalInvoiceXmlDeserilizationException($"Message document is empty. It should contain an XML document. Message Id='{envelope.MessageId}'.");
                    }
                    var data = Convert.FromBase64String(envelope.Message.Document);
                    using MemoryStream stream = new(data);
                    XDocument xml = XDocument.Load(stream);
                    if (xml.Root?.Name?.LocalName == "Comprobante" && xml.Root?.Name?.Namespace == MxNamespaces.Cfdi)
                    {
                        XmlSerializer serializer = new(typeof(Voucher));
                        var reader = xml.CreateReader();
                        reader.MoveToContent();
                        var voucher = (Voucher?)serializer.Deserialize(reader);
                        if (voucher == null)
                        {
                            throw new MexicanDigitalInvoiceXmlDeserilizationException($"Error while deserializing Mexican digital invoice voucher in {nameof(DataIntegrationSubmitted)} message '{envelope.MessageId}' :\n" + xml.ToString());
                        }
                        string uuid = voucher.Issuer?.Code + "-" + voucher.InvoiceId;
                        MexicanDigitalInvoiceState state = new();
                        MexicanDigitalInvoice mexicanInvoice = new(uuid, state);
                        var events = await mexicanInvoice.Submit(voucher);
                        await _repository.SetState(uuid, envelope.ToMetadata(), state, cancellationToken);
                        await _repository.Publish(events.Select(p => new Envelope(p, new Hexalith.Domain.ValueTypes.MessageId(), envelope)).ToList(), cancellationToken);
                        await _repository.Save(cancellationToken);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error while handling mexican digital invoice integration. MessageId='{envelope.MessageId}'; Handler='{GetType().FullName}'.");
                    throw;
                }
            }
        }

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
            => Handle(new Envelope<DataIntegrationSubmitted>(envelope), cancellationToken);
    }
}