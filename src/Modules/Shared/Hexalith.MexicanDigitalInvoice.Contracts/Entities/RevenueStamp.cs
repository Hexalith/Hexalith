namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("TimbreFiscalDigital", Namespace = MxNamespaces.Tfd)]
    [XmlRoot("TimbreFiscalDigital", Namespace = MxNamespaces.Tfd)]
    public class RevenueStamp
    {
        [DataMember(Order = 1), ProtoMember(2)]
        [XmlAttribute()]
        public string? UUID { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlAttribute]
        public string? Version { get; set; }
    }
}