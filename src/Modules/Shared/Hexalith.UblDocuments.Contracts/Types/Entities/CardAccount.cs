namespace Hexalith.UblDocuments.Types.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Hexalith.UblDocuments.Helpers;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlRoot(Namespace = UblNamespaces.CommonAggregateComponents2)]
    [XmlType(Namespace = UblNamespaces.CommonAggregateComponents2)]
    public class CardAccount
    {
        [DataMember(Order = 8, IsRequired = true), ProtoMember(9, IsRequired = true)]
        [XmlElement(Order = 8, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? CardChipCode { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? CardTypeCode { get; set; }

        [DataMember(Order = 9, IsRequired = true), ProtoMember(10, IsRequired = true)]
        [XmlElement(Order = 9, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ChipApplicationID { get; set; }

        [DataMember(Order = 7, IsRequired = true), ProtoMember(8, IsRequired = true)]
        [XmlElement(Order = 7, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? CV2ID { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 4, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ExpiracyDate { get; set; }

        [DataMember(Order = 4, IsRequired = true), ProtoMember(5, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? ExpiracyDateTime
        {
            get => ExpiracyDate.ToNullableDateTime();
            set => ExpiracyDate = value.ToDateString();
        }

        [DataMember(Order = 10, IsRequired = true), ProtoMember(11, IsRequired = true)]
        [XmlElement(Order = 10, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? HolderName { get; set; }

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? IssuerID { get; set; }

        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement(Order = 6, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? IssuerNumberID { get; set; }

        [DataMember(Order = 1, IsRequired = true), ProtoMember(2, IsRequired = true)]
        [XmlElement(Order = 1, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string NetworkID { get; set; } = string.Empty;

        [DataMember(Order = 0, IsRequired = true), ProtoMember(1, IsRequired = true)]
        [XmlElement(Order = 0, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string PrimaryAccountNumberID { get; set; } = string.Empty;

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 3, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ValidityDate { get; set; }

        [DataMember(Order = 3, IsRequired = true), ProtoMember(4, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? ValidityDateTime
        {
            get => ValidityDate.ToNullableDateTime();
            set => ValidityDate = value.ToDateString();
        }
    }
}