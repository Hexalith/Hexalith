namespace Hexalith.Emails.Contracts.ValueTypes
{
    using System.Diagnostics;

    using ProtoBuf;

    [DebuggerDisplay("{Name}")]
    [ProtoContract]
    public class Attachment
    {
        [ProtoMember(2)]
        public string Content { get; set; } = string.Empty;

        [ProtoMember(1)]
        public string Name { get; set; } = string.Empty;
    }
}