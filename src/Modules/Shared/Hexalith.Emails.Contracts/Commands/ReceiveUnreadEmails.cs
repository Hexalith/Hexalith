namespace Hexalith.Emails.Application.Commands
{
    using Hexalith.Domain.Contracts.Commands;

    using ProtoBuf;

    [Command]
    [ProtoContract(SkipConstructor = true)]
    public sealed class ReceiveUnreadEmails
    {
        [ProtoMember(2)]
        public string Recipient { get; set; } = string.Empty;
    }
}