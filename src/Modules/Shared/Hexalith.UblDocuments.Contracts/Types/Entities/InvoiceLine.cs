namespace Hexalith.UblDocuments.Types.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Text.Json;
    using System.Xml.Serialization;

    using Hexalith.UblDocuments.Helpers;
    using Hexalith.UblDocuments.Types.Aggregates;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlRoot(Namespace = UblNamespaces.CommonAggregateComponents2)]
    [XmlType(Namespace = UblNamespaces.CommonAggregateComponents2)]
    public class InvoiceLine
    {
        [DataMember(Order = 7), ProtoMember(8)]
        [XmlElement(Order = 7, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? AccountingCost { get; set; }

        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement(Order = 6, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? AccountingCostCode { get; set; }

        [NotMapped]
        [DataMember(Order = 20), ProtoMember(21)]
        [XmlElement(Order = 20, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<AllowanceCharge> AllowanceCharge { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 14), ProtoMember(15)]
        [XmlElement(Order = 14, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<BillingReference> BillingReference { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 18), ProtoMember(19)]
        [XmlElement(Order = 18, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<Delivery> Delivery { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 25), ProtoMember(26)]
        [XmlElement(Order = 25, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public DeliveryTerms DeliveryTerms { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 12), ProtoMember(13)]
        [XmlElement(Order = 12, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<LineReference> DespatchLineReference { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 15), ProtoMember(16)]
        [XmlElement(Order = 15, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<DocumentReference> DocumentReference { get; set; } = new();

        [DataMember(Order = 9), ProtoMember(10)]
        [XmlElement(Order = 9, Namespace = UblNamespaces.CommonBasicComponents2)]
        public bool FreeOfChargeIndicator { get; set; }

        [DataMember(Order = 0, IsRequired = true), ProtoMember(1, IsRequired = true)]
        [XmlElement(Order = 0, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string ID { get; set; } = string.Empty;

        [IgnoreDataMember]
        [XmlIgnore]
        public Invoice Invoice { get; set; } = new();

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement(Order = 3, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal InvoicedQuantity { get; set; }

        [IgnoreDataMember]
        [XmlIgnore]
        public int InvoiceId { get; set; }

        [Key]
        [IgnoreDataMember]
        [XmlIgnore]
        public int InvoiceLineId { get; set; }

        [DataMember(Order = 10), ProtoMember(11)]
        [XmlElement(Order = 10, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Period InvoicePeriod { get; set; } = new();

        [DataMember(Order = 23, IsRequired = true), ProtoMember(24, IsRequired = true)]
        [XmlElement(Order = 23, IsNullable = false, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Item Item { get; set; } = new();

        [DataMember(Order = 4, IsRequired = true), ProtoMember(5, IsRequired = true)]
        [XmlElement(Order = 4, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal LineExtensionAmount { get; set; }

        [NotMapped]
        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public List<string> Note { get; set; } = new();

        [IgnoreDataMember]
        [XmlIgnore]
        public string NoteText
        {
            get => JsonSerializer.Serialize(Note);
            set => Note = JsonSerializer.Deserialize<List<string>>(value) ?? new();
        }

        [DataMember(Order = 11), ProtoMember(12)]
        [XmlElement(Order = 11, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<OrderLineReference> OrderLineReference { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 17), ProtoMember(18)]
        [XmlElement(Order = 17, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Party OriginatorParty { get; set; } = new();

        [DataMember(Order = 8), ProtoMember(9)]
        [XmlElement(Order = 8, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PaymentPurposeCode { get; set; }

        [NotMapped]
        [DataMember(Order = 19), ProtoMember(20)]
        [XmlElement(Order = 19, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<PaymentTerms> PaymentTerms { get; set; } = new();

        [DataMember(Order = 24, IsRequired = false), ProtoMember(25, IsRequired = true)]
        [XmlElement(Order = 24, IsNullable = true, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Price Price { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 16), ProtoMember(17)]
        [XmlElement(Order = 16, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public PricingReference PricingReference { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 13), ProtoMember(14)]
        [XmlElement(Order = 13, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<LineReference> ReceiptLineReference { get; set; } = new();

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 5, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? TaxPointDate { get; set; }

        [DataMember(Order = 5, IsRequired = true), ProtoMember(6, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? TaxPointDateTime
        {
            get => TaxPointDate.ToNullableDateTime();
            set => TaxPointDate = value.ToDateString();
        }

        [NotMapped]
        [DataMember(Order = 21), ProtoMember(22)]
        [XmlElement(Order = 21, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<TaxTotal> TaxTotal { get; set; } = new();

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? UUID { get; set; }

        [NotMapped]
        [DataMember(Order = 22), ProtoMember(23)]
        [XmlElement(Order = 22, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<TaxTotal> WithholdingTaxTotal { get; set; } = new();
    }
}