namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Entry", Namespace = MxNamespaces.Fx)]
    [XmlRoot("Entry", Namespace = MxNamespaces.Fx)]
    public class DictionaryEntry
    {
        [DataMember(Order = 0), ProtoMember(1)]
        [XmlAttribute("k")]
        public string Key { get; set; } = string.Empty;

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlAttribute("v")]
        public string? Value { get; set; }
    }
}