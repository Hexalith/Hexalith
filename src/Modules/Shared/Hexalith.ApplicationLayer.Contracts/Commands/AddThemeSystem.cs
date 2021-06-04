namespace Hexalith.ApplicationLayer.Commands
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Hexalith.Domain.Contracts.Commands;

    using ProtoBuf;

    [ProtoContract]
    [DataContract]
    [Command]
    public class AddThemeSystem
    {
        [ProtoMember(3)]
        [DataMember(Order = 2)]
        public List<string> Scripts { get; set; } = new();

        [ProtoMember(2)]
        [DataMember(Order = 1)]
        public List<string> Stylesheets { get; set; } = new();

        [ProtoMember(1)]
        [DataMember(Order = 0)]
        public string ThemeSystemName { get; set; } = string.Empty;
    }
}