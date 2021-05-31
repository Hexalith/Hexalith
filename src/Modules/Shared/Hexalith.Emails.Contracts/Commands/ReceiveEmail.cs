namespace Hexalith.Emails.Application.Commands
{
    using System;
    using System.Collections.Generic;

    using Hexalith.Domain.Contracts.Commands;
    using Hexalith.Emails.Contracts.ValueTypes;

    using ProtoBuf;

    [Command]
    [ProtoContract(SkipConstructor = true)]
    public sealed class ReceiveEmail
    {
        [ProtoMember(100)]
        public IEnumerable<Attachment> Attachments { get; set; } = Array.Empty<Attachment>();

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