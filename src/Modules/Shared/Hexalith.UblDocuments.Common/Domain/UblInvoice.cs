namespace Hexalith.UblDocuments.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hexalith.UblDocuments.Domain.States;
    using Hexalith.UblDocuments.Events;
    using Hexalith.UblDocuments.Types.Aggregates;

    public class UblInvoice
    {
        private readonly IUblInvoiceState _state;

        public UblInvoice(string id, IUblInvoiceState state)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            _state = state;
        }

        public Task<IEnumerable<object>> Submit(
            Invoice invoice)
        {
            List<object> events = new();
            events.Add(new UblInvoiceSubmitted
            {
                Invoice = invoice
            });
            _state.Apply(events);
            return Task.FromResult<IEnumerable<object>>(events);
        }
    }
}