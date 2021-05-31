namespace Hexalith.MexicanDigitalInvoice.Domain.States
{
    using System.Collections.Generic;

    using Hexalith.MexicanDigitalInvoice.Aggregates;

    public interface IMexicanDigitalInvoiceState
    {
        Voucher Invoice { get; set; }

        void Apply(IEnumerable<object> events);
    }
}