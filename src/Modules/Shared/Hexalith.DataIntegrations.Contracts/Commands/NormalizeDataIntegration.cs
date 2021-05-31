namespace Hexalith.DataIntegrations.Contracts.Commands
{
    using Hexalith.Domain.Contracts.Commands;

    using ProtoBuf;

    [Command]
    [ProtoContract(SkipConstructor = true)]
    public sealed class NormalizeDataIntegration
    {
        [ProtoMember(1)]
        public string DataIntegrationId { get; set; } = string.Empty;
    }
}