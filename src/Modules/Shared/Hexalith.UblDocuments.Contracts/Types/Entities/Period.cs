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
    public class Period
    {
        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement(Order = 6, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? Description { get; set; }

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? DescriptionCode { get; set; }

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement(Order = 4, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal? DurationMeasure { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 2, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? EndDate { get; set; }

        [DataMember(Order = 2, IsRequired = true), ProtoMember(3, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? EndDateTime
        {
            get => (EndDate, EndTime).ToNullableDateTime();
            set => (EndDate, EndTime) = value.ToDateTimeStrings();
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 3, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? EndTime { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 0, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? StartDate { get; set; }

        [DataMember(Order = 0, IsRequired = true), ProtoMember(1, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? StartDateTime
        {
            get => (StartDate, StartTime).ToNullableDateTime();
            set => (StartDate, StartTime) = value.ToDateTimeStrings();
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? StartTime { get; set; }
    }
}