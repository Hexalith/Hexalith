namespace Hexalith.ApplicationLayer.Events
{
    using System.Runtime.Serialization;

    using Hexalith.Domain.Contracts.Commands;

    using ProtoBuf;

    [ProtoContract]
    [DataContract]
    [Command]
    public class UserInterfaceThemeChanged
    {
        [ProtoMember(1)]
        [DataMember(Order = 0)]
        public string ThemeName { get; set; } = default!;

        [ProtoMember(2)]
        [DataMember(Order = 1)]
        public string UserName { get; set; } = default!;
    }
}