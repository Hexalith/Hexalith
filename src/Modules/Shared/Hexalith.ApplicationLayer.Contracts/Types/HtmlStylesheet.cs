namespace Hexalith.ApplicationLayer.Types
{
    using System.Runtime.Serialization;

    using ProtoBuf;

    [ProtoContract]
    [DataContract]
    public class HtmlStylesheet
    {
        [DataMember(Order = 5)]
        [ProtoMember(4)]
        public string CrossOrigin { get; set; } = string.Empty;

        [DataMember(Order = 1)]
        [ProtoMember(2)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        [ProtoMember(3)]
        public string Link { get; set; } = string.Empty;

        [DataMember(Order = 0)]
        [ProtoMember(1)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        [ProtoMember(3)]
        public string ReferrerPolicy { get; set; } = string.Empty;

        [DataMember(Order = 10)]
        [ProtoMember(9)]
        public string StyleContent { get; set; } = string.Empty;
    }
}