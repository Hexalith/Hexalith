namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Dictionary", Namespace = MxNamespaces.Fx)]
    [XmlRoot("Dictionary", Namespace = MxNamespaces.Fx)]
    public class DictionarySet
    {
        [XmlElement("Entry", Order = 0), ProtoMember(1)]
        public List<DictionaryEntry> Entry { get; set; } = new();

        [DataMember(Order = 100), ProtoMember(101)]
        [XmlAttribute("name")]
        public string? Name { get; set; } = string.Empty;
    }
}