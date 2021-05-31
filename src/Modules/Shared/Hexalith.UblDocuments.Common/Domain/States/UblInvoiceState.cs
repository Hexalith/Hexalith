namespace Hexalith.UblDocuments.Domain.States
{
    using Hexalith.UblDocuments.Events;
    using Hexalith.UblDocuments.Types.Aggregates;

    using System;
    using System.Collections.Generic;

    public sealed class UblInvoiceState : IUblInvoiceState
    {

        public void Apply(IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case UblInvoiceSubmitted submitted:
                        Apply(submitted);
                        break;

                    default:
                        throw new NotSupportedException($"Event type '{@event.GetType().Name} is not supported by '{nameof(UblInvoiceState)}''");
                }
            }
        }

        private void Apply(UblInvoiceSubmitted submitted)
        {
            Invoice = submitted.Invoice;
        }

        public Invoice Invoice { get; set; } = new();
    }
}