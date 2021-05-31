using ProtoBuf;

namespace Hexalith.DataIntegrations.Contracts.ValueTypes
{
    [ProtoContract]
    public class DataIntegrationField
    {
        [ProtoMember(3)]
        public bool Mandatory { get; set; }

        [ProtoMember(1)]
        public string Name { get; set; } = string.Empty;

        [ProtoMember(2)]
        public string Type { get; set; } = string.Empty;
    }
}