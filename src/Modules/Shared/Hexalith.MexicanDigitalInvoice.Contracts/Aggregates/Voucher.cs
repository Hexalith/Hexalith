namespace Hexalith.MexicanDigitalInvoice.Aggregates
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    using Hexalith.MexicanDigitalInvoice.Entities;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Comprobante", Namespace = MxNamespaces.Cfdi)]
    [XmlRoot("Comprobante", Namespace = MxNamespaces.Cfdi)]
    public class Voucher
    {
        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement("Addenda", Order = 5, Namespace = MxNamespaces.Cfdi)]
        public Addendum? Addendum { get; set; }

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement("Complemento", Order = 4, Namespace = MxNamespaces.Cfdi)]
        public Complement? Complement { get; set; }

        [DataMember(Order = 101), ProtoMember(102)]
        [XmlAttribute("Moneda")]
        public string? Currency { get; set; }

        [XmlAttribute("Fecha")]
        [ProtoIgnore]
        [IgnoreDataMember]
        [NotMapped]
        public string DocumentDate
        {
            get => XmlConvert.ToString(DocumentDateTime.LocalDateTime, XmlDateTimeSerializationMode.Local);
            set => DocumentDateTime = new DateTimeOffset(XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Unspecified), new TimeSpan(-6, 0, 0));
        }

        [DataMember(Order = 102), ProtoMember(103)]
        [XmlIgnore]
        public DateTimeOffset DocumentDateTime { get; set; }

        [DataMember(Order = 100), ProtoMember(101)]
        [XmlAttribute("Folio")]
        public string? InvoiceId { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement("Emisor", Order = 0, Namespace = MxNamespaces.Cfdi)]
        public Issuer? Issuer { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement("Receptor", Order = 1, Namespace = MxNamespaces.Cfdi)]
        public Receiver? Receiver { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement("Impuestos", Order = 3, Namespace = MxNamespaces.Cfdi)]
        public Tax? Tax { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement("Conceptos", Order = 2, Namespace = MxNamespaces.Cfdi)]
        public TaxItems? TaxItems { get; set; }
    }
}