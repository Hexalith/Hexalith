using ProtoBuf;

namespace Hexalith.DataIntegrations.Contracts.ModelViews
{
    [ProtoContract]
    public sealed class DataIntegrationDetails
    {
        [ProtoMember(1)]
        public string DataIntegrationId { get; set; } = string.Empty;

        [ProtoMember(5)]
        public string Description { get; set; } = string.Empty;

        [ProtoMember(3)]
        public string DocumentName { get; set; } = string.Empty;

        [ProtoMember(4)]
        public string DocumentType { get; set; } = string.Empty;

        [ProtoMember(2)]
        public string Name { get; set; } = string.Empty;
    }
}