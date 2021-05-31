namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Identificacion", Namespace = MxNamespaces.Fx)]
    [XmlRoot("Identificacion", Namespace = MxNamespaces.Fx)]
    public class Identification
    {
        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement("LugarExpedicion", Order = 5, Namespace = MxNamespaces.Fx)]
        public string? DeliveryLocation { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement("TipoDeComprobante", Order = 1, Namespace = MxNamespaces.Fx)]
        public string? DocumentType { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement("RFCEmisor", Order = 2, Namespace = MxNamespaces.Fx)]
        public string? IssuerCode { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement("CdgPaisEmisor", Order = 0, Namespace = MxNamespaces.Fx)]
        public string? IssuerCountryCode { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement("RazonSocialEmisor", Order = 3, Namespace = MxNamespaces.Fx)]
        public string? IssuerName { get; set; }

        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement("CadenaOriginal", Order = 6, Namespace = MxNamespaces.Fx)]
        public string? Signature { get; set; }

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement("Usuario", Order = 4, Namespace = MxNamespaces.Fx)]
        public string? UserCode { get; set; }
    }
}