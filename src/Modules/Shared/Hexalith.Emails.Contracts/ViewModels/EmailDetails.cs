namespace Hexalith.Emails.Application.ModelViews
{
    using System;
    using System.Collections.Generic;

    using ProtoBuf;

    [ProtoContract]
    public class EmailDetailedInformations
    {
        [ProtoMember(8)]
        public IEnumerable<string> AttachmentNames { get; set; } = Array.Empty<string>();

        [ProtoMember(7)]
        public string Body { get; set; } = string.Empty;

        [ProtoMember(5)]
        public IEnumerable<string> CopyToRecipients { get; set; } = Array.Empty<string>();

        [ProtoMember(1)]
        public string EmailId { get; set; } = string.Empty;

        [ProtoMember(2)]
        public string Recipient { get; set; } = string.Empty;

        [ProtoMember(3)]
        public string Sender { get; set; } = string.Empty;

        [ProtoMember(6)]
        public string Subject { get; set; } = string.Empty;

        [ProtoMember(4)]
        public IEnumerable<string> ToRecipients { get; set; } = Array.Empty<string>();
    }
}