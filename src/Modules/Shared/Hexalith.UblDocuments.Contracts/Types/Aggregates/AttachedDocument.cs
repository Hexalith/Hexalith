namespace Hexalith.UblDocuments.Types.Aggregates
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Hexalith.UblDocuments.Helpers;
    using Hexalith.UblDocuments.Types.Entities;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType(Namespace = UblNamespaces.AttachedDocument2)]
    [XmlRoot(Namespace = UblNamespaces.AttachedDocument2)]
    public class AttachedDocument
    {
        [DataMember(Order = 18, IsRequired = true), ProtoMember(19, IsRequired = true)]
        [XmlElement(Order = 18, IsNullable = false, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Attachment Attachment { get; set; } = new();

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? CustomizationID { get; set; } = string.Empty;

        [DataMember(Order = 11), ProtoMember(12)]
        [XmlElement(Order = 11, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? DocumentType { get; set; } = string.Empty;

        [DataMember(Order = 10), ProtoMember(11)]
        [XmlElement(Order = 10, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? DocumentTypeCode { get; set; } = string.Empty;

        [DataMember(Order = 5, IsRequired = true), ProtoMember(6, IsRequired = true)]
        [XmlElement(Order = 5, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string ID { get; set; } = string.Empty;

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 7, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? IssueDate { get; set; }

        [DataMember(Order = 7, IsRequired = true), ProtoMember(8, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? IssueDateTime
        {
            get => (IssueDate, IssueTime).ToNullableDateTime();
            set => (IssueDate, IssueTime) = value.ToDateTimeStrings();
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 8, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? IssueTime { get; set; }

        [DataMember(Order = 9), ProtoMember(10)]
        [XmlElement(Order = 9, Namespace = UblNamespaces.CommonBasicComponents2)]
        public List<string> Note { get; set; } = new();

        [DataMember(Order = 12), ProtoMember(13)]
        [XmlElement(Order = 12, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ParentDocumentID { get; set; } = string.Empty;

        [DataMember(Order = 19), ProtoMember(20)]
        [XmlElement(Order = 19, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public LineReference? ParentDocumentLineReference { get; set; }

        [DataMember(Order = 13), ProtoMember(14)]
        [XmlElement(Order = 13, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ParentDocumentTypeCode { get; set; } = string.Empty;

        [DataMember(Order = 14), ProtoMember(15)]
        [XmlElement(Order = 14, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ParentDocumentVersionID { get; set; } = string.Empty;

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement(Order = 4, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ProfileExecutionID { get; set; } = string.Empty;

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement(Order = 3, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ProfileID { get; set; } = string.Empty;

        [DataMember(Order = 17, IsRequired = true), ProtoMember(18, IsRequired = true)]
        [XmlElement(Order = 17, IsNullable = false, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Party ReceiverParty { get; set; } = new();

        [DataMember(Order = 16, IsRequired = true), ProtoMember(17, IsRequired = true)]
        [XmlElement(Order = 16, IsNullable = false, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Party SenderParty { get; set; } = new();

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? UBLExtensions { get; set; } = string.Empty;

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? UBLVersionID { get; set; } = string.Empty;

        [DataMember(Order = 6, IsRequired = true), ProtoMember(7, IsRequired = true)]
        [XmlElement(Order = 6, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? UUID { get; set; } = string.Empty;
    }
}