using Hexalith.Domain.Contracts.Events;

using ProtoBuf;

namespace Hexalith.DataIntegrations.Contracts.Events
{
    [Event]
    [ProtoContract]
    public sealed class DataIntegrationNormalized
    {
        [ProtoMember(4, IsRequired = true)]
        public dynamic Data { get; set; } = string.Empty;

        [ProtoMember(1, IsRequired = true)]
        public string DataIntegrationId { get; set; } = string.Empty;

        [ProtoMember(3)]
        public string Description { get; set; } = string.Empty;

        [ProtoMember(2)]
        public string Name { get; set; } = string.Empty;
    }
}