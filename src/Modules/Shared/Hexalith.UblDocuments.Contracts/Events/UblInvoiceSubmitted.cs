namespace Hexalith.UblDocuments.Events
{
    using Hexalith.Domain.Contracts.Events;
    using Hexalith.UblDocuments.Types.Aggregates;

    using ProtoBuf;

    [Event]
    [ProtoContract]
    public sealed class UblInvoiceSubmitted
    {
        [ProtoMember(1)]
        public Invoice Invoice { get; set; } = new();
    }
}