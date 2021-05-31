namespace Hexalith.UblDocuments.Types.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlRoot(Namespace = UblNamespaces.CommonAggregateComponents2)]
    [XmlType(Namespace = UblNamespaces.CommonAggregateComponents2)]
    public class Address
    {
        [DataMember(Order = 7), ProtoMember(8)]
        [XmlElement(Order = 7, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? AdditionalStreetName { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? AddressFormatCode { get; set; }

        [DataMember(Order = 24), ProtoMember(25)]
        [XmlElement(Order = 24, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<AddressLine> AddressLine { get; set; } = new();

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? AddressTypeCode { get; set; }

        [DataMember(Order = 8), ProtoMember(9)]
        [XmlElement(Order = 8, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? BlockName { get; set; }

        [DataMember(Order = 9), ProtoMember(10)]
        [XmlElement(Order = 9, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? BuildingName { get; set; }

        [DataMember(Order = 10), ProtoMember(11)]
        [XmlElement(Order = 10, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? BuildingNumber { get; set; }

        [DataMember(Order = 17), ProtoMember(18)]
        [XmlElement(Order = 17, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? CityName { get; set; }

        [DataMember(Order = 16), ProtoMember(17)]
        [XmlElement(Order = 16, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? CitySubdivisionName { get; set; }

        [DataMember(Order = 25), ProtoMember(26)]
        [XmlElement(Order = 25, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Country? Country { get; set; }

        [DataMember(Order = 19), ProtoMember(20)]
        [XmlElement(Order = 19, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? CountrySubentity { get; set; }

        [DataMember(Order = 20), ProtoMember(21)]
        [XmlElement(Order = 20, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? CountrySubentityCode { get; set; }

        [DataMember(Order = 12), ProtoMember(13)]
        [XmlElement(Order = 12, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? Department { get; set; }

        [DataMember(Order = 22), ProtoMember(23)]
        [XmlElement(Order = 22, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? District { get; set; }

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement(Order = 4, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? Floor { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ID { get; set; }

        [DataMember(Order = 11), ProtoMember(12)]
        [XmlElement(Order = 11, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? InhouseMail { get; set; }

        [DataMember(Order = 26), ProtoMember(27)]
        [XmlElement(Order = 26, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<LocationCoordinate> LocationCoordinate { get; set; } = new();

        [DataMember(Order = 13), ProtoMember(14)]
        [XmlElement(Order = 13, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? MarkAttention { get; set; }

        [DataMember(Order = 14), ProtoMember(15)]
        [XmlElement(Order = 14, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? MarkCare { get; set; }

        [DataMember(Order = 15), ProtoMember(16)]
        [XmlElement(Order = 15, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PlotIdentification { get; set; }

        [DataMember(Order = 18), ProtoMember(19)]
        [XmlElement(Order = 18, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PostalZone { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement(Order = 3, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? Postbox { get; set; }

        [DataMember(Order = 21), ProtoMember(22)]
        [XmlElement(Order = 21, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? Region { get; set; }

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? Room { get; set; }

        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement(Order = 6, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? StreetName { get; set; }

        [DataMember(Order = 23), ProtoMember(24)]
        [XmlElement(Order = 23, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? TimezoneOffset { get; set; }
    }
}