namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Complemento", Namespace = MxNamespaces.Cfdi)]
    [XmlRoot("Complemento", Namespace = MxNamespaces.Cfdi)]
    public class Complement
    {
        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement("TimbreFiscalDigital", Order = 0, Namespace = MxNamespaces.Tfd)]
        public RevenueStamp? RevenueStamp { get; set; }
    }
}