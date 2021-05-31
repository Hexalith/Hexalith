namespace Hexalith.SalesHistory.Application.Events
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Events;
    using Hexalith.Application.Messages;
    using Hexalith.SalesHistory.Common.Application.Services;
    using Hexalith.SalesHistory.Common.Application.States;
    using Hexalith.UblDocuments.Events;

    using Microsoft.Extensions.Logging;

    [EventHandler(Event = typeof(UblInvoiceSubmitted))]
    public class UblInvoiceSubmittedHandler : IEventHandler<UblInvoiceSubmitted>
    {
        private readonly ILogger<UblInvoiceSubmittedHandler> _logger;
        private readonly ISalesHistoryRepository _repository;

        public UblInvoiceSubmittedHandler(ISalesHistoryRepository repository, ILogger<UblInvoiceSubmittedHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
           => Handle(new Envelope<UblInvoiceSubmitted>(envelope), cancellationToken);

        public async Task Handle(Envelope<UblInvoiceSubmitted> envelope, CancellationToken cancellationToken = default)
        {
            try
            {
                var invoice = envelope.Message.Invoice;
                await _repository.AddSales(
                invoice.InvoiceLine
                    .Select(p =>
                        new SalesHistoryState()
                        {
                            CompanyId = invoice.AccountingSupplierParty.Party?.PartyIdentification.FirstOrDefault()?.ID ?? string.Empty,
                            CompanyName = invoice.AccountingSupplierParty.Party?.PartyName.Name ?? string.Empty,
                            CustomerId = invoice.AccountingCustomerParty.Party?.PartyIdentification.FirstOrDefault()?.ID ?? string.Empty,
                            CustomerName = invoice.AccountingCustomerParty.Party?.PartyName.Name ?? string.Empty,
                            Currency = invoice.DocumentCurrencyCode ?? string.Empty,
                            InvoiceDate = invoice.IssueDateTime,
                            InvoiceId = invoice.ID ?? string.Empty,
                            SalesId = string.Empty,
                            LineId = p.ID,
                            ItemId = p.Item.SellersItemIdentification?.ID ?? string.Empty,
                            ItemName = p.Item.Description,
                            Quantity = p.InvoicedQuantity,
                            TotalAmount = p.LineExtensionAmount
                        }
                    ), cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error while integrating UBL Invoice in sales history : {e.Message}.\nMessageId={envelope.MessageId}; Company={envelope.Message.Invoice.AccountingSupplierParty.Party?.PartyName.Name}; InvoiceId={envelope.Message.Invoice.ID}.");
            }
        }
    }
}