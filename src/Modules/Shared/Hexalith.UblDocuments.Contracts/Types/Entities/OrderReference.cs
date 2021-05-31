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
    public class OrderReference
    {
        [DataMember(Order = 2, IsRequired = true), ProtoMember(3, IsRequired = true)]
        [XmlElement(Order = 2, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public bool CopyIndicator { get; set; }

        [DataMember(Order = 6, IsRequired = true), ProtoMember(7, IsRequired = true)]
        [XmlElement(Order = 6, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? CustomerReference { get; set; }

        [DataMember(Order = 8, IsRequired = true), ProtoMember(9, IsRequired = true)]
        [XmlElement(Order = 8, IsNullable = false, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public DocumentReference? DocumentReference { get; set; }

        [DataMember(Order = 0, IsRequired = true), ProtoMember(1, IsRequired = true)]
        [XmlElement(Order = 0, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string ID { get; set; } = string.Empty;

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 4, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? IssueDate { get; set; }

        [DataMember(Order = 4, IsRequired = true), ProtoMember(5, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? IssueDateTime
        {
            get => (IssueDate, IssueTime).ToNullableDateTime();
            set => (IssueDate, IssueTime) = value.ToDateTimeStrings();
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? IssueTime { get; set; }

        [DataMember(Order = 7, IsRequired = true), ProtoMember(8, IsRequired = true)]
        [XmlElement(Order = 7, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? OrderTypeCode { get; set; }

        [DataMember(Order = 1, IsRequired = true), ProtoMember(2, IsRequired = true)]
        [XmlElement(Order = 1, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? SalesOrderID { get; set; }

        [DataMember(Order = 3, IsRequired = true), ProtoMember(4, IsRequired = true)]
        [XmlElement(Order = 3, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? UUID { get; set; }
    }
}