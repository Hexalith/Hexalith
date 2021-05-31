namespace Hexalith.MexicanDigitalInvoice.Events
{
    using Hexalith.Domain.Contracts.Events;
    using Hexalith.MexicanDigitalInvoice.Aggregates;

    using ProtoBuf;

    [Event]
    [ProtoContract]
    public sealed class MexicanDigitalInvoiceSubmitted
    {
        [ProtoMember(1)]
        public Voucher Invoice { get; set; } = new();
    }
}