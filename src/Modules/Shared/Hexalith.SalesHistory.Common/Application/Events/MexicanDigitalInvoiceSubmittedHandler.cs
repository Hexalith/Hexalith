namespace Hexalith.SalesHistory.Application.Events
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Events;
    using Hexalith.Application.Messages;
    using Hexalith.MexicanDigitalInvoice.Events;
    using Hexalith.SalesHistory.Application.Exceptions;
    using Hexalith.SalesHistory.Common.Application.Services;
    using Hexalith.SalesHistory.Common.Application.States;

    using Microsoft.Extensions.Logging;

    [EventHandler(Event = typeof(MexicanDigitalInvoiceSubmitted))]
    public class MexicanDigitalInvoiceSubmittedHandler : IEventHandler<MexicanDigitalInvoiceSubmitted>
    {
        private readonly ILogger<MexicanDigitalInvoiceSubmittedHandler> _logger;
        private readonly ISalesHistoryRepository _repository;

        public MexicanDigitalInvoiceSubmittedHandler(ISalesHistoryRepository repository, ILogger<MexicanDigitalInvoiceSubmittedHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
           => Handle(new Envelope<MexicanDigitalInvoiceSubmitted>(envelope), cancellationToken);

        public async Task Handle(Envelope<MexicanDigitalInvoiceSubmitted> envelope, CancellationToken cancellationToken = default)
        {
            try
            {
                var invoice = envelope.Message.Invoice.Addendum?.Invoice;
                if (invoice == null)
                {
                    throw new MexicanDigitalInvoiceDeserializationException($"Missing invoice document in Xml file. MessageId='{envelope.MessageId}'.");
                }
                int i = 0;
                await _repository.AddSales(
                invoice.InvoiceLines
                    .Select(p =>
                    {
                        var parts = p.Description?.Split(' ', 2);
                        var itemId = parts?.FirstOrDefault() ?? string.Empty;
                        var lineId = Convert.ToString(++i, CultureInfo.InvariantCulture) ?? string.Empty;
                        return new SalesHistoryState()
                        {
                            CompanyId = invoice.Identification?.IssuerCode ?? string.Empty,
                            CompanyName = invoice.Identification?.IssuerName ?? string.Empty,
                            CustomerId = invoice.Customer?.Code ?? string.Empty,
                            CustomerName = invoice.Customer?.Name ?? string.Empty,
                            Currency = envelope.Message.Invoice.Currency ?? string.Empty,
                            InvoiceDate = envelope.Message.Invoice.DocumentDateTime,
                            InvoiceId = envelope.Message.Invoice.InvoiceId ?? string.Empty,
                            SalesId = string.Empty,
                            LineId = lineId,
                            ItemId = itemId,
                            ItemName = p.Description,
                            Quantity = p.Quantity,
                            TotalAmount = p.LineAmount
                        };
                    }), cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Mexican digital invoice integration in sales history error : {e.Message}.\nMessageId={envelope.MessageId}; Company={envelope.Message.Invoice.Addendum?.Invoice?.Identification?.IssuerName}; InvoiceId={envelope.Message.Invoice.InvoiceId}.");
            }
        }
    }
}