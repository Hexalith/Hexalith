namespace Hexalith.ApplicationLayer.Commands
{
    using System.Runtime.Serialization;

    using Hexalith.Domain.Contracts.Commands;

    using ProtoBuf;

    [ProtoContract]
    [DataContract]
    [Command]
    public class AddStylesheetToThemeSystem
    {
        [ProtoMember(3)]
        [DataMember(Order = 2)]
        public int? AfterPosition { get; set; }

        [ProtoMember(2)]
        [DataMember(Order = 1)]
        public string Stylesheet { get; set; } = string.Empty;

        [ProtoMember(1)]
        [DataMember(Order = 0)]
        public string ThemeSystemName { get; set; } = string.Empty;
    }
}