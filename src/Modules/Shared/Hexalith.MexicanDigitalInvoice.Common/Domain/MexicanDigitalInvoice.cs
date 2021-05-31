namespace Hexalith.MexicanDigitalInvoice.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hexalith.MexicanDigitalInvoice.Aggregates;
    using Hexalith.MexicanDigitalInvoice.Domain.States;
    using Hexalith.MexicanDigitalInvoice.Events;

    public class MexicanDigitalInvoice
    {
        private readonly IMexicanDigitalInvoiceState _state;

        public MexicanDigitalInvoice(string id, IMexicanDigitalInvoiceState state)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));
            _state = state;
        }

        public Task<IEnumerable<object>> Submit(
            Voucher invoice)
        {
            List<object> events = new();
            events.Add(new MexicanDigitalInvoiceSubmitted
            {
                Invoice = invoice
            });
            _state.Apply(events);
            return Task.FromResult<IEnumerable<object>>(events);
        }
    }
}