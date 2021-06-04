namespace Hexalith.ApplicationLayer.Commands
{
    using System.Runtime.Serialization;

    using Hexalith.Domain.Contracts.Commands;

    using ProtoBuf;

    [ProtoContract]
    [DataContract]
    [Command]
    public class RemoveThemeSystem
    {
        [ProtoMember(1)]
        [DataMember(Order = 0)]
        public string Name { get; set; } = string.Empty;

        [ProtoMember(2)]
        [DataMember(Order = 1)]
        public string ReplaceBy { get; set; } = string.Empty;
    }
}