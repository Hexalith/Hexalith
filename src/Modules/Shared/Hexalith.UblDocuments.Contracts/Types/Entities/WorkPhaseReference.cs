namespace Hexalith.UblDocuments.Types.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Hexalith.UblDocuments.Helpers;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlRoot(Namespace = UblNamespaces.CommonAggregateComponents2)]
    [XmlType(Namespace = UblNamespaces.CommonAggregateComponents2)]
    public class WorkPhaseReference
    {
        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 4, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? EndDate { get; set; }

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlIgnore]
        public DateTimeOffset? EndDateTime
        {
            get => EndDate.ToNullableDateTime();
            set => EndDate = value.ToDateString();
        }

        [DataMember(Order = 0, IsRequired = true), ProtoMember(1, IsRequired = true)]
        [XmlElement(Order = 0, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ID { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal? ProgressPercent { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 3, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? StartDate { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlIgnore]
        public DateTimeOffset? StartDateTime
        {
            get => StartDate.ToNullableDateTime();
            set => StartDate = value.ToDateString();
        }

        [DataMember(Order = 5, IsRequired = true), ProtoMember(6, IsRequired = true)]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<DocumentReference>? WorkOrderDocumentReference { get; set; } = new();

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? WorkPhaseCode { get; set; }
    }
}