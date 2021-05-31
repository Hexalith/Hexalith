namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Receptor", Namespace = MxNamespaces.Cfdi)]
    [XmlRoot("Receptor", Namespace = MxNamespaces.Cfdi)]
    public class Receiver
    {
        [DataMember(Order = 0), ProtoMember(1)]
        [XmlAttribute("Rfc")]
        public string? Code { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlAttribute("Nombre")]
        public string? Name { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlAttribute("UsoCFDI")]
        public string? UsageCode { get; set; }
    }
}