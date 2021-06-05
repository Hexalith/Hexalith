namespace Hexalith.ApplicationLayer.Types
{
    using System.Runtime.Serialization;

    using ProtoBuf;

    [ProtoContract]
    [DataContract]
    public class HtmlScript
    {
        [DataMember(Order = 10)]
        [ProtoMember(9)]
        public bool Async { get; set; }

        [DataMember(Order = 9)]
        [ProtoMember(8)]
        public string CrossOrigin { get; set; } = string.Empty;

        [DataMember(Order = 8)]
        [ProtoMember(7)]
        public bool Defer { get; set; }

        [DataMember(Order = 1)]
        [ProtoMember(2)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 7)]
        [ProtoMember(6)]
        public string Intigrity { get; set; } = string.Empty;

        [DataMember(Order = 0)]
        [ProtoMember(1)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 5)]
        [ProtoMember(4)]
        public bool NoModule { get; set; }

        [DataMember(Order = 6)]
        [ProtoMember(5)]
        public string ReferrerPolicy { get; set; } = string.Empty;

        [DataMember(Order = 11)]
        [ProtoMember(10)]
        public string ScriptContent { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        [ProtoMember(3)]
        public string ScriptType { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        [ProtoMember(3)]
        public string Source { get; set; } = string.Empty;
    }
}