namespace Hexalith.UblDocuments.Application.Events
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
    using Hexalith.UblDocuments.Domain;
    using Hexalith.UblDocuments.Domain.States;
    using Hexalith.UblDocuments.Types;
    using Hexalith.UblDocuments.Types.Aggregates;

    using Microsoft.Extensions.Logging;

    [EventHandler(Event = typeof(DataIntegrationSubmitted))]
    public class DataIntegrationSubmittedHandler : IEventHandler<DataIntegrationSubmitted>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<DataIntegrationSubmittedHandler> _logger;
        private readonly IRepository<IUblInvoiceState> _repository;

        public DataIntegrationSubmittedHandler(
            IRepository<IUblInvoiceState> repository,
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
                if (string.IsNullOrWhiteSpace(envelope.Message.Document))
                {
                    throw new UblXmlDeserilizationException($"Message document is empty. It should contain an XML document. Message Id='{envelope.MessageId}'.");
                }
                try
                {
                    var data = Convert.FromBase64String(envelope.Message.Document);
                    using MemoryStream stream = new(data);
                    XDocument xml = XDocument.Load(stream);
                    if (xml.Root?.Name?.LocalName == nameof(AttachedDocument) && xml.Root?.Name?.Namespace == UblNamespaces.AttachedDocument2)
                    {
                        xml = UblAttachedDocument(envelope, xml);
                    }

                    if (xml.Root?.Name?.LocalName == nameof(Invoice) && xml.Root?.Name?.Namespace == UblNamespaces.Invoice2)
                    {
                        XmlSerializer serializer = new(typeof(Invoice));
                        var reader = xml.CreateReader();
                        reader.MoveToContent();
                        var invoice = (Invoice?)serializer.Deserialize(reader);
                        if (invoice == null)
                        {
                            throw new UblXmlDeserilizationException($"Error while deserializing UBL Invoice in {nameof(DataIntegrationSubmitted)} message '{envelope.MessageId}' :\n" + xml.ToString());
                        }
                        if (string.IsNullOrWhiteSpace(invoice.UUID))
                        {
                            throw new UblXmlDeserilizationException($"Error while deserializing UBL Invoice in {nameof(DataIntegrationSubmitted)}. The UUID field is empty. Message '{envelope.MessageId}' :\n" + xml.ToString());
                        }
                        if (await _repository.Exists(invoice.UUID, cancellationToken))
                        {
                            _logger.LogWarning($"Duplicate UBL Invoice. UUID='{invoice.UUID}',  ID='{invoice.ID}'. Message '{envelope.MessageId}'");
                        }
                        UblInvoiceState state = new();
                        UblInvoice ublInvoice = new(invoice.UUID, state);
                        var events = await ublInvoice.Submit(invoice);
                        await _repository.AddStateLog(invoice.UUID, envelope.ToMetadata(), events, cancellationToken);
                        await _repository.SetState(invoice.UUID, envelope.ToMetadata(), state, cancellationToken);
                        await _repository.Publish(events.Select(p => new Envelope(p, new Hexalith.Domain.ValueTypes.MessageId(), envelope)).ToList(), cancellationToken);
                        await _repository.Save(cancellationToken);
                        return;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error while handling integration messages in '{GetType().FullName}'. MessageId={envelope.MessageId}.");
                    throw;
                }
            }
        }

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
            => Handle(new Envelope<DataIntegrationSubmitted>(envelope), cancellationToken);

        private static XDocument UblAttachedDocument(Envelope<DataIntegrationSubmitted> envelope, XDocument xml)
        {
            XmlSerializer serializer = new(typeof(AttachedDocument));
            var doc = (AttachedDocument?)serializer.Deserialize(xml.CreateReader());
            if (doc == null)
            {
                throw new UblXmlDeserilizationException($"Error while deserializing UBL xml document in {nameof(DataIntegrationSubmitted)} message '{envelope.MessageId}' :\n" + xml.ToString());
            }

            var content = doc.Attachment?.ExternalReference?.Description;
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new UblXmlDeserilizationException($"AttachedDocument.Attachment.ExternalReference.Description is empty. It should contain the UBL Invoice XML. Message Id='{envelope.MessageId}' :\n" + xml.ToString());
            }

            return XDocument.Parse(content);
        }
    }
}