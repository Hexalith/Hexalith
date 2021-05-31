using ProtoBuf;

namespace Hexalith.DataIntegrations.Contracts.ModelViews
{
    [ProtoContract]
    public sealed class DataIntegrationSummary
    {
        [ProtoMember(1)]
        public string DataIntegrationId { get; set; } = string.Empty;

        [ProtoMember(3)]
        public string DocumentName { get; set; } = string.Empty;

        [ProtoMember(2)]
        public string Name { get; set; } = string.Empty;
    }
}