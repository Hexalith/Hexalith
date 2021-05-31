namespace Hexalith.MexicanDigitalInvoice.Domain.States
{
    using System;
    using System.Collections.Generic;

    using Hexalith.MexicanDigitalInvoice.Aggregates;
    using Hexalith.MexicanDigitalInvoice.Events;

    public sealed class MexicanDigitalInvoiceState : IMexicanDigitalInvoiceState
    {
        public Voucher Invoice { get; set; } = new();

        public void Apply(IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case MexicanDigitalInvoiceSubmitted submitted:
                        Apply(submitted);
                        break;

                    default:
                        throw new NotSupportedException($"Event type '{@event.GetType().Name} is not supported by '{nameof(MexicanDigitalInvoiceState)}''");
                }
            }
        }

        private void Apply(MexicanDigitalInvoiceSubmitted submitted)
        {
            Invoice = submitted.Invoice;
        }
    }
}