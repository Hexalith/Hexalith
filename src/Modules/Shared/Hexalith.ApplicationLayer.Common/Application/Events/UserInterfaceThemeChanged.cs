namespace Hexalith.ApplicationLayer.Application.Events
{
    using System.Runtime.Serialization;

    using Hexalith.Domain.Contracts.Events;

    using ProtoBuf;

    [Event]
    [DataContract]
    [ProtoContract]
    internal class UserInterfaceThemeChanged
    {
        [DataMember(Order = 2)]
        [ProtoMember(3)]
        public string NewThemeName { get; set; } = string.Empty;

        [DataMember(Order = 1)]
        [ProtoMember(2)]
        public string OldThemeName { get; set; } = string.Empty;

        [DataMember(Order = 0)]
        [ProtoMember(1)]
        public string UserName { get; set; } = string.Empty;
    }
}