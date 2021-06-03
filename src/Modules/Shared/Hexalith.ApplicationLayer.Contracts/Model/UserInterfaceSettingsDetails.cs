namespace Hexalith.ApplicationLayer.Model
{
    using System.Runtime.Serialization;

    using ProtoBuf;

    [ProtoContract]
    [DataContract]
    public class UserInterfaceSettingsDetails
    {
        [ProtoMember(1)]
        [DataMember(Order = 0)]
        public string ThemeName { get; set; } = default!;
    }
}