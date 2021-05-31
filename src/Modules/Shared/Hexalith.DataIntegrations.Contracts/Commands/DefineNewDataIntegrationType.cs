namespace Hexalith.DataIntegrations.Contracts.Commands
{
    using System;
    using System.Collections.Generic;

    using Hexalith.DataIntegrations.Contracts.ValueTypes;
    using Hexalith.Domain.Contracts.Commands;

    using ProtoBuf;

    [Command]
    [ProtoContract(SkipConstructor = true)]
    public sealed class DefineNewDataIntegrationType
    {
        [ProtoMember(1)]
        public string DataIntegrationTypeId { get; set; } = string.Empty;

        [ProtoMember(3)]
        public string Description { get; set; } = string.Empty;

        [ProtoMember(4)]
        public IEnumerable<DataIntegrationField> Fields { get; set; } = Array.Empty<DataIntegrationField>();

        [ProtoMember(2)]
        public string Name { get; set; } = string.Empty;
    }
}