namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Traslado", Namespace = MxNamespaces.Cfdi)]
    [XmlRoot("Traslado", Namespace = MxNamespaces.Cfdi)]
    public class TaxTransaction
    {
        [DataMember(Order = 4), ProtoMember(5)]
        [XmlAttribute("Importe")]
        public decimal Amount { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlAttribute("Base")]
        public decimal Base { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlAttribute("TipoFactor")]
        public string? FactorType { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlAttribute("TasaOCuota")]
        public decimal Percent { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlAttribute("Impuesto")]
        public string? TaxCode { get; set; }
    }
}