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
    public class PrepaidPayment
    {
        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ID { get; set; }

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? InstructionID { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal PaidAmount { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 3, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PaidDate { get; set; }

        [DataMember(Order = 3, IsRequired = true), ProtoMember(4, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? PaidDateTime
        {
            get => (PaidDate, PaidTime).ToNullableDateTime();
            set => (PaidDate, PaidTime) = value.ToDateTimeStrings();
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 4, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PaidTime { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 2, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ReceivedDate { get; set; }

        [DataMember(Order = 2, IsRequired = true), ProtoMember(3, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? ReceivedDateTime
        {
            get => ReceivedDate.ToNullableDateTime();
            set => ReceivedDate = value.ToDateString();
        }
    }
}