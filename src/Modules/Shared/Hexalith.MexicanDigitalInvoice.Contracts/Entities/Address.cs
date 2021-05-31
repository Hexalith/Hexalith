namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("DomicilioMexicano", Namespace = MxNamespaces.Fx)]
    [XmlRoot("DomicilioMexicano", Namespace = MxNamespaces.Fx)]
    public class Address
    {
        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement("NumeroInterior", Order = 2)]
        public string? AppartmentNumber { get; set; }

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement("Municipio", Order = 4)]
        public string? City { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement("Colonia", Order = 3)]
        public string? CityLocation { get; set; }

        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement("Pais", Order = 6)]
        public string? Country { get; set; }

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement("Estado", Order = 5)]
        public string? CountryLocation { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement("Calle", Order = 0)]
        public string? Street { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement("NumeroExterior", Order = 1)]
        public string? StreetNumber { get; set; }

        [DataMember(Order = 7), ProtoMember(8)]
        [XmlElement("CodigoPostal", Order = 7)]
        public string? ZipCode { get; set; }
    }
}