namespace Hexalith.UblDocuments.Types.Aggregates
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Hexalith.UblDocuments.Helpers;
    using Hexalith.UblDocuments.Types.Entities;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType(Namespace = UblNamespaces.Invoice2)]
    [XmlRoot(Namespace = UblNamespaces.Invoice2)]
    public class Invoice : EntityBase
    {
        [DataMember(Order = 20), ProtoMember(21)]
        [XmlElement(Order = 20, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? AccountingCost { get; set; } = string.Empty;

        [DataMember(Order = 19), ProtoMember(20)]
        [XmlElement(Order = 19, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public string? AccountingCostCode { get; set; }

        [NotMapped]
        [DataMember(Order = 35, IsRequired = true), ProtoMember(36, IsRequired = true)]
        [XmlElement(Order = 35, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public AccountingCustomerParty AccountingCustomerParty { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 34, IsRequired = true), ProtoMember(35, IsRequired = true)]
        [XmlElement(Order = 34, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public SupplierParty AccountingSupplierParty { get; set; } = new();

        [DataMember(Order = 31), ProtoMember(32)]
        [XmlElement(Order = 31, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<DocumentReference> AdditionalDocumentReference { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 45), ProtoMember(46)]
        [XmlElement(Order = 45, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<AllowanceCharge> AllowanceCharge { get; set; } = new();

        [DataMember(Order = 25), ProtoMember(26)]
        [XmlElement(Order = 25, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<BillingReference> BillingReference { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 37), ProtoMember(38)]
        [XmlElement(Order = 37, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Party BuyerCustomerParty { get; set; } = new();

        [DataMember(Order = 22), ProtoMember(23)]
        [XmlElement(Order = 22, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? BuyerReference { get; set; } = string.Empty;

        [DataMember(Order = 30), ProtoMember(31)]
        [XmlElement(Order = 30, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<DocumentReference> ContractDocumentReference { get; set; } = new();

        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement(Order = 6, Namespace = UblNamespaces.CommonBasicComponents2)]
        public bool CopyIndicator { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? CustomizationID { get; set; } = string.Empty;

        [NotMapped]
        [DataMember(Order = 40), ProtoMember(41)]
        [XmlElement(Order = 40, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<Delivery> Delivery { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 41), ProtoMember(42)]
        [XmlElement(Order = 41, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public DeliveryTerms DeliveryTerms { get; set; } = new();

        [DataMember(Order = 26), ProtoMember(27)]
        [XmlElement(Order = 26, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public DocumentReference DespatchDocumentReference { get; set; } = new();

        [DataMember(Order = 14), ProtoMember(15)]
        [XmlElement(Order = 14, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? DocumentCurrencyCode { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 10, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? DueDate { get; set; }

        [DataMember(Order = 10), ProtoMember(11)]
        [XmlIgnore]
        public DateTimeOffset? DueDateTime
        {
            get => DueDate.ToNullableDateTime();
            set => DueDate = value.ToDateString();
        }

        [DataMember(Order = 5, IsRequired = true), ProtoMember(6, IsRequired = true)]
        [XmlElement(Order = 5, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ID { get; set; } = string.Empty;

        [Key]
        [XmlIgnore]
        [IgnoreDataMember]
        public int InvoiceId { get; set; }

        [DataMember(Order = 53, IsRequired = true, Name = nameof(InvoiceLine)), ProtoMember(54, IsRequired = true)]
        [XmlElement(Order = 53, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<InvoiceLine> InvoiceLine { get; set; } = new();

        [DataMember(Order = 23, IsRequired = true), ProtoMember(24, IsRequired = true)]
        [XmlElement(Order = 23, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Period InvoicePeriod { get; set; } = new();

        [DataMember(Order = 11), ProtoMember(12)]
        [XmlElement(Order = 11, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? InvoiceTypeCode { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 8, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string IssueDate { get; set; } = string.Empty;

        [DataMember(Order = 9, IsRequired = true), ProtoMember(10, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset IssueDateTime
        {
            get => (IssueDate, IssueTime).ToDateTime();
            set => (IssueDate, IssueTime) = value.ToDateTimeStrings();
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 9, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? IssueTime { get; set; }

        [DataMember(Order = 52, IsRequired = true), ProtoMember(53, IsRequired = true)]
        [XmlElement(Order = 52, IsNullable = false, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public LegalMonetaryTotal LegalMonetaryTotal { get; set; } = new();

        [DataMember(Order = 21), ProtoMember(22)]
        [XmlElement(Order = 21, Namespace = UblNamespaces.CommonBasicComponents2)]
        public int LineCountNumeric { get; set; }

        [DataMember(Order = 12), ProtoMember(13)]
        [XmlElement(Order = 12, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? Note { get; set; }

        [DataMember(Order = 24), ProtoMember(25)]
        [XmlElement(Order = 24, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public OrderReference OrderReference { get; set; } = new();

        [DataMember(Order = 29), ProtoMember(30)]
        [XmlElement(Order = 29, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<DocumentReference> OriginatorDocumentReference { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 36), ProtoMember(37)]
        [XmlElement(Order = 36, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Party PayeeParty { get; set; } = new();

        [DataMember(Order = 18), ProtoMember(19)]
        [XmlElement(Order = 18, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PaymentAlternativeCurrencyCode { get; set; }

        [DataMember(Order = 49), ProtoMember(50)]
        [XmlElement(Order = 49, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal PaymentAlternativeExchangeRate { get; set; }

        [DataMember(Order = 17), ProtoMember(18)]
        [XmlElement(Order = 17, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PaymentCurrencyCode { get; set; }

        [DataMember(Order = 48), ProtoMember(49)]
        [XmlElement(Order = 48, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal PaymentExchangeRate { get; set; }

        [NotMapped]
        [DataMember(Order = 42), ProtoMember(43)]
        [XmlElement(Order = 42, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<PaymentMeans> PaymentMeans { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 43), ProtoMember(44)]
        [XmlElement(Order = 43, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<PaymentTerms> PaymentTerms { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 44), ProtoMember(45)]
        [XmlElement(Order = 44, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<PrepaidPayment> PrepaidPayment { get; set; } = new();

        [DataMember(Order = 16), ProtoMember(17)]
        [XmlElement(Order = 16, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? PricingCurrencyCode { get; set; }

        [DataMember(Order = 47), ProtoMember(48)]
        [XmlElement(Order = 47, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal PricingExchangeRate { get; set; }

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement(Order = 4, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ProfileExecutionID { get; set; } = string.Empty;

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement(Order = 3, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? ProfileID { get; set; } = string.Empty;

        [NotMapped]
        [DataMember(Order = 32), ProtoMember(33)]
        [XmlElement(Order = 32, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<ProjectReference> ProjectReference { get; set; } = new();

        [DataMember(Order = 27), ProtoMember(28)]
        [XmlElement(Order = 27, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<DocumentReference> ReceiptDocumentReference { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 38), ProtoMember(39)]
        [XmlElement(Order = 38, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public SupplierParty SellerSupplierParty { get; set; } = new();

        [DataMember(Order = 28), ProtoMember(29)]
        [XmlElement(Order = 28, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<DocumentReference> StatementDocumentReference { get; set; } = new();

        [DataMember(Order = 15), ProtoMember(16)]
        [XmlElement(Order = 15, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? TaxCurrencyCode { get; set; }

        [DataMember(Order = 46), ProtoMember(47)]
        [XmlElement(Order = 46, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal TaxExchangeRate { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [XmlElement(Order = 13, IsNullable = false, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? TaxPointDate { get; set; }

        [DataMember(Order = 13, IsRequired = true), ProtoMember(14, IsRequired = true)]
        [XmlIgnore]
        public DateTimeOffset? TaxPointDateTime
        {
            get => TaxPointDate.ToNullableDateTime();
            set => TaxPointDate = value.ToDateString();
        }

        [NotMapped]
        [DataMember(Order = 39), ProtoMember(40)]
        [XmlElement(Order = 39, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Party TaxRepresentativeParty { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 51), ProtoMember(52)]
        [XmlElement(Order = 51, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<TaxTotal> TaxTotal { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0, Namespace = UblNamespaces.CommonExtensionComponents2)]
        public UBLExtensions? UBLExtensions { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? UBLVersionID { get; set; } = string.Empty;

        [DataMember(Order = 7), ProtoMember(8)]
        [XmlElement(Order = 7, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? UUID { get; set; }
    }
}