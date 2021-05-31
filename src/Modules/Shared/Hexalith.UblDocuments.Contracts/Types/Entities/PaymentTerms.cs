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
    public class PaymentTerms
    {
        [DataMember(Order = 8), ProtoMember(9)]
        [XmlElement(Order = 8, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal Amount { get; set; }

        [DataMember(Order = 17), ProtoMember(18)]
        [XmlElement(Order = 17, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public ExchangeRate? ExchangeRate { get; set; }

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ID { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 13, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? InstallmentDueDate { get; set; }

        [DataMember(Order = 13, IsRequired = true), ProtoMember(14, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? InstallmentDueDateTime
        {
            get => InstallmentDueDate.ToNullableDateTime();
            set => InstallmentDueDate = value.ToDateString();
        }

        [DataMember(Order = 14), ProtoMember(15)]
        [XmlElement(Order = 14, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? InvoicingPartyReference { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement(Order = 3, Namespace = UblNamespaces.CommonBasicComponents2)]
        public List<string> Note { get; set; } = new();

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 12, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PaymentDueDate { get; set; }

        [DataMember(Order = 12, IsRequired = true), ProtoMember(13, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? PaymentDueDateTime
        {
            get => PaymentDueDate.ToNullableDateTime();
            set => PaymentDueDate = value.ToDateString();
        }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public List<string> PaymentMeansID { get; set; } = new();

        [DataMember(Order = 7), ProtoMember(8)]
        [XmlElement(Order = 7, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal PaymentPercent { get; set; }

        [DataMember(Order = 11), ProtoMember(12)]
        [XmlElement(Order = 11, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PaymentTermsDetailsURI { get; set; }

        [DataMember(Order = 10), ProtoMember(11)]
        [XmlElement(Order = 10, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal PenaltyAmount { get; set; }

        [DataMember(Order = 16), ProtoMember(17)]
        [XmlElement(Order = 16, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Period? PenaltyPeriod { get; set; }

        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement(Order = 6, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal PenaltySurchargePercent { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PrepaidPaymentReferenceID { get; set; }

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement(Order = 4, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ReferenceEventCode { get; set; }

        [DataMember(Order = 9), ProtoMember(10)]
        [XmlElement(Order = 9, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal SettlementDiscountAmount { get; set; }

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal SettlementDiscountPercent { get; set; }

        [DataMember(Order = 15), ProtoMember(16)]
        [XmlElement(Order = 15, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Period? SettlementPeriod { get; set; }

        [DataMember(Order = 18), ProtoMember(19)]
        [XmlElement(Order = 18, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Period? ValidityPeriod { get; set; }
    }
}