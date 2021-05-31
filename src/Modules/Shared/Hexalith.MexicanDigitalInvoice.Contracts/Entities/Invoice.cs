namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("FactDocMX", Namespace = MxNamespaces.Fx)]
    [XmlRoot("FactDocMX", Namespace = MxNamespaces.Fx)]
    public class Invoice
    {
        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement("Receptor", Order = 4, Namespace = MxNamespaces.Fx)]
        public Customer? Customer { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement("Identificacion", Order = 1, Namespace = MxNamespaces.Fx)]
        public Identification? Identification { get; set; }

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlArray("Conceptos", Order = 5, Namespace = MxNamespaces.Fx)]
        [XmlArrayItem("Concepto", Namespace = MxNamespaces.Fx)]
        public List<InvoiceLine> InvoiceLines { get; set; } = new();

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement("Procesamiento", Order = 2, Namespace = MxNamespaces.Fx)]
        public Processing? Processing { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement("Emisor", Order = 3, Namespace = MxNamespaces.Fx)]
        public Seller? Seller { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0, Namespace = MxNamespaces.Fx)]
        public int Version { get; set; }
    }
}