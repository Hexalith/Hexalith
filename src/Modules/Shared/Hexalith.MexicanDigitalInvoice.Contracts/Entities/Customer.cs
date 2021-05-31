namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Receptor", Namespace = MxNamespaces.Fx)]
    [XmlRoot("Receptor", Namespace = MxNamespaces.Fx)]
    public class Customer
    {
        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement("RFCReceptor", Order = 1)]
        public string? Code { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement("CdgPaisReceptor", Order = 0)]
        public string? CountryCode { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement("NombreReceptor", Order = 2)]
        public string? Name { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement("Domicilio", Order = 3)]
        public Residence? Residence { get; set; }

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement("UsoCFDI", Order = 4)]
        public string? UsageCode { get; set; }
    }
}