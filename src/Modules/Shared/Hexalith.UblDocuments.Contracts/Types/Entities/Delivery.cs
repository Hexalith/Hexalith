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
    public class Delivery
    {
        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 4, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ActualDeliveryDate { get; set; }

        [DataMember(Order = 4, IsRequired = true), ProtoMember(5, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? ActualDeliveryDateTime
        {
            get => (ActualDeliveryDate, ActualDeliveryTime).ToNullableDateTime();
            set => (ActualDeliveryDate, ActualDeliveryTime) = value.ToDateTimeStrings();
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ActualDeliveryTime { get; set; }

        [DataMember(Order = 12), ProtoMember(13)]
        [XmlElement(Order = 12, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Location? AlternativeDeliveryLocation { get; set; }

        [DataMember(Order = 16), ProtoMember(17)]
        [XmlElement(Order = 16, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Party? CarrierParty { get; set; }

        [DataMember(Order = 10), ProtoMember(11)]
        [XmlElement(Order = 10, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Address? DeliveryAddress { get; set; }

        [DataMember(Order = 11), ProtoMember(12)]
        [XmlElement(Order = 11, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Location? DeliveryLocation { get; set; }

        [DataMember(Order = 17), ProtoMember(18)]
        [XmlElement(Order = 17, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Party? DeliveryParty { get; set; }

        [DataMember(Order = 20), ProtoMember(21)]
        [XmlElement(Order = 20, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public DeliveryTerms? DeliveryTerms { get; set; }

        [DataMember(Order = 15), ProtoMember(16)]
        [XmlElement(Order = 15, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Period? EstimatedDeliveryPeriod { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ID { get; set; } = string.Empty;

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 6, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? LatestDeliveryDate { get; set; }

        [DataMember(Order = 6, IsRequired = true), ProtoMember(7, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? LatestDeliveryDateTime
        {
            get => (LatestDeliveryDate, LatestDeliveryTime).ToNullableDateTime();
            set => (LatestDeliveryDate, LatestDeliveryTime) = value.ToDateTimeStrings();
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 7, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? LatestDeliveryTime { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement(Order = 3, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal MaximumQuantity { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal MinimumQuantity { get; set; }

        [DataMember(Order = 18), ProtoMember(19)]
        [XmlElement(Order = 18, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Party? NotifyParty { get; set; }

        [DataMember(Order = 14), ProtoMember(15)]
        [XmlElement(Order = 14, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Period? PromisedDeliveryPeriod { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal Quantity { get; set; }

        [DataMember(Order = 8), ProtoMember(9)]
        [XmlElement(Order = 8, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ReleaseID { get; set; } = string.Empty;

        [DataMember(Order = 13), ProtoMember(14)]
        [XmlElement(Order = 13, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Period? RequestedDeliveryPeriod { get; set; }

        [DataMember(Order = 9), ProtoMember(10)]
        [XmlElement(Order = 9, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? TrackingID { get; set; } = string.Empty;
    }
}