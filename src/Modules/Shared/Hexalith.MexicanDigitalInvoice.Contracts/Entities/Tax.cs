namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Impuestos", Namespace = MxNamespaces.Cfdi)]
    [XmlRoot("Impuestos", Namespace = MxNamespaces.Cfdi)]
    public class Tax
    {
        [DataMember(Order = 100), ProtoMember(101)]
        [XmlAttribute("TotalImpuestosTrasladados")]
        public decimal Total { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement("Traslados", Order = 0)]
        public TaxTransactions? Transactions { get; set; }
    }
}