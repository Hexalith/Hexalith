using Hexalith.UblDocuments.Types.Aggregates;

using System.Collections.Generic;

namespace Hexalith.UblDocuments.Domain.States
{
    public interface IUblInvoiceState
    {
        Invoice Invoice { get; set; }

        void Apply(IEnumerable<object> events);
    }
}