using ProtoBuf;

namespace Hexalith.DataIntegrations.Contracts.ValueTypes
{
    [ProtoContract]
    public class DataIntegrationFieldMap
    {
        [ProtoMember(1)]
        public string Name { get; set; } = string.Empty;

        [ProtoMember(2)]
        public string Value { get; set; } = string.Empty;
    }
}