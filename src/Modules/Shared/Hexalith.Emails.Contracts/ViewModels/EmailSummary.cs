using ProtoBuf;

namespace Hexalith.Emails.Application.ModelViews
{
    [ProtoContract]
    public sealed class EmailSummaryInformations
    {
        [ProtoMember(1)]
        public string EmailId { get; set; } = string.Empty;

        [ProtoMember(2)]
        public string Recipient { get; set; } = string.Empty;

        [ProtoMember(3)]
        public string Sender { get; set; } = string.Empty;

        [ProtoMember(4)]
        public string Subject { get; set; } = string.Empty;
    }
}