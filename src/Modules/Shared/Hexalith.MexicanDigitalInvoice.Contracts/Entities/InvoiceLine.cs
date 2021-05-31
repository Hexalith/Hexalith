namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Concepto", Namespace = MxNamespaces.Fx)]
    [XmlRoot("Concepto", Namespace = MxNamespaces.Fx)]
    public class InvoiceLine
    {
        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement("Descripcion", Order = 4, Namespace = MxNamespaces.Fx)]
        public string? Description { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement("ClaveUnidad", Order = 1, Namespace = MxNamespaces.Fx)]
        public string? KeyUnit { get; set; }

        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement("Importe", Order = 6, Namespace = MxNamespaces.Fx)]
        public decimal LineAmount { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement("UnidadDeMedida", Order = 2, Namespace = MxNamespaces.Fx)]
        public string? MesureUnit { get; set; }

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement("ValorUnitario", Order = 5, Namespace = MxNamespaces.Fx)]
        public decimal Price { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement("ClaveProdServ", Order = 3, Namespace = MxNamespaces.Fx)]
        public string? ProductService { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement("Cantidad", Order = 0, Namespace = MxNamespaces.Fx)]
        public decimal Quantity { get; set; }
    }
}