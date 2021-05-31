namespace Hexalith.UblDocuments.Types.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlRoot(Namespace = UblNamespaces.CommonAggregateComponents2)]
    [XmlType(Namespace = UblNamespaces.CommonAggregateComponents2)]
    public class ExternalReference
    {
        [DataMember(Order = 8), ProtoMember(9)]
        [XmlElement(Order = 8, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string CharacterSetCode { get; set; } = string.Empty;

        [DataMember(Order = 10), ProtoMember(11)]
        [XmlElement(Order = 10, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string DocumentHash { get; set; } = string.Empty;

        [DataMember(Order = 7), ProtoMember(8)]
        [XmlElement(Order = 7, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string EncodingCode { get; set; } = string.Empty;

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement(Order = 3, Namespace = UblNamespaces.CommonBasicComponents2)]
        public DateTimeOffset ExpiryDate { get; set; }

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement(Order = 4, Namespace = UblNamespaces.CommonBasicComponents2)]
        public DateTime ExpiryTime { get; set; }

        [DataMember(Order = 9), ProtoMember(10)]
        [XmlElement(Order = 9, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string FileName { get; set; } = string.Empty;

        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement(Order = 6, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string FormatCode { get; set; } = string.Empty;

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string HashAlgorithmMethod { get; set; } = string.Empty;

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string MimeCode { get; set; } = string.Empty;

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string URI { get; set; } = string.Empty;
    }
}