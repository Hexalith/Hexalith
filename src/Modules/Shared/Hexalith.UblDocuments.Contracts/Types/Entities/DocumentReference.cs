namespace Hexalith.UblDocuments.Types.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Hexalith.UblDocuments.Helpers;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlRoot(Namespace = UblNamespaces.CommonAggregateComponents2)]
    [XmlType(Namespace = UblNamespaces.CommonAggregateComponents2)]
    public class DocumentReference
    {
        [DataMember(Order = 13), ProtoMember(14)]
        [XmlElement(Order = 13, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Attachment Attachment { get; set; } = new();

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public bool CopyIndicator { get; set; }

        [DataMember(Order = 12), ProtoMember(13)]
        [XmlElement(Order = 12, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? DocumentDescription { get; set; }

        [Key]
        [XmlIgnore]
        [IgnoreDataMember]
        public int DocumentReferenceId { get; set; }

        [DataMember(Order = 11), ProtoMember(12)]
        [XmlElement(Order = 11, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? DocumentStatusCode { get; set; }

        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement(Order = 6, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? DocumentType { get; set; }

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? DocumentTypeCode { get; set; }

        [DataMember(Order = 0, IsRequired = true), ProtoMember(1, IsRequired = true)]
        [XmlElement(Order = 0, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string ID { get; set; } = string.Empty;

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 3, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? IssueDate { get; set; }

        [DataMember(Order = 3, IsRequired = true), ProtoMember(4, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? IssueDateTime
        {
            get => (IssueDate, IssueTime).ToNullableDateTime();
            set => (IssueDate, IssueTime) = value.ToDateTimeStrings();
        }

        [NotMapped]
        [DataMember(Order = 15, IsRequired = true), ProtoMember(16, IsRequired = true)]
        [XmlElement(Order = 15, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Party? IssuerParty { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 4, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? IssueTime { get; set; }

        [DataMember(Order = 8), ProtoMember(9)]
        [XmlElement(Order = 8, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? LanguageID { get; set; }

        [DataMember(Order = 9), ProtoMember(10)]
        [XmlElement(Order = 9, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? LocaleCode { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? UUID { get; set; }

        [DataMember(Order = 14, IsRequired = true), ProtoMember(15, IsRequired = true)]
        [XmlElement(Order = 14, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Period? ValidityPeriod { get; set; }

        [DataMember(Order = 10), ProtoMember(11)]
        [XmlElement(Order = 10, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? VersionID { get; set; }

        [DataMember(Order = 7, IsRequired = true), ProtoMember(8, IsRequired = true)]
        [XmlElement(Order = 7, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? XPath { get; set; }
    }
}