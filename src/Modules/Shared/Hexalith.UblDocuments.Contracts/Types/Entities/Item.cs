namespace Hexalith.UblDocuments.Types.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlRoot(Namespace = UblNamespaces.CommonAggregateComponents2)]
    [XmlType(Namespace = UblNamespaces.CommonAggregateComponents2)]
    public class Item
    {
        [NotMapped]
        [DataMember(Order = 6), ProtoMember(7)]
        [XmlElement(Order = 6, Namespace = UblNamespaces.CommonBasicComponents2)]
        public List<string> AdditionalInformation { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 15), ProtoMember(16)]
        [XmlElement(Order = 15, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<ItemIdentification> AdditionalItemIdentification { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 23), ProtoMember(24)]
        [XmlElement(Order = 23, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<ItemProperty> AdditionalItemProperty { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 8), ProtoMember(9)]
        [XmlElement(Order = 8, Namespace = UblNamespaces.CommonBasicComponents2)]
        public List<string> BrandName { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 10), ProtoMember(11)]
        [XmlElement(Order = 10, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public ItemIdentification? BuyersItemIdentification { get; set; }

        [NotMapped]
        [DataMember(Order = 16), ProtoMember(17)]
        [XmlElement(Order = 16, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public DocumentReference? CatalogueDocumentReference { get; set; }

        [DataMember(Order = 3), ProtoMember(4)]
        [XmlElement(Order = 3, Namespace = UblNamespaces.CommonBasicComponents2)]
        public bool CatalogueIndicator { get; set; }

        [NotMapped]
        [DataMember(Order = 14), ProtoMember(15)]
        [XmlElement(Order = 14, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public ItemIdentification? CatalogueItemIdentification { get; set; }

        [NotMapped]
        [DataMember(Order = 22), ProtoMember(23)]
        [XmlElement(Order = 22, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<TaxCategory> ClassifiedTaxCategory { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 19), ProtoMember(20)]
        [XmlElement(Order = 19, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<CommodityClassification> CommodityClassification { get; set; } = new();

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? Description { get; set; }

        [DataMember(Order = 5), ProtoMember(6)]
        [XmlElement(Order = 5, Namespace = UblNamespaces.CommonBasicComponents2)]
        public bool HazardousRiskIndicator { get; set; }

        [Key]
        [IgnoreDataMember]
        [XmlIgnore]
        public int ItemId { get; set; }

        [NotMapped]
        [DataMember(Order = 17), ProtoMember(18)]
        [XmlElement(Order = 17, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<DocumentReference> ItemSpecificationDocumentReference { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 7), ProtoMember(8)]
        [XmlElement(Order = 7, Namespace = UblNamespaces.CommonBasicComponents2)]
        public List<string> Keyword { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 24), ProtoMember(25)]
        [XmlElement(Order = 24, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<Party> ManufacturerParty { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 12), ProtoMember(13)]
        [XmlElement(Order = 12, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<ItemIdentification> ManufacturersItemIdentification { get; set; } = new();

        [NotMapped]
        [DataMember(Order = 9), ProtoMember(10)]
        [XmlElement(Order = 9, Namespace = UblNamespaces.CommonBasicComponents2)]
        public List<string> ModelName { get; set; } = new();

        [DataMember(Order = 4), ProtoMember(5)]
        [XmlElement(Order = 4, Namespace = UblNamespaces.CommonBasicComponents2)]
        public string? Name { get; set; }

        [NotMapped]
        [DataMember(Order = 18), ProtoMember(19)]
        [XmlElement(Order = 18, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public Country? OriginCountry { get; set; }

        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal PackQuantity { get; set; }

        [DataMember(Order = 2), ProtoMember(3)]
        [XmlElement(Order = 2, Namespace = UblNamespaces.CommonBasicComponents2)]
        public decimal PackSizeNumeric { get; set; }

        [NotMapped]
        [DataMember(Order = 11), ProtoMember(12)]
        [XmlElement(Order = 11, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public ItemIdentification? SellersItemIdentification { get; set; }

        [NotMapped]
        [DataMember(Order = 13), ProtoMember(14)]
        [XmlElement(Order = 13, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public ItemIdentification? StandardItemIdentification { get; set; }

        [NotMapped]
        [DataMember(Order = 20), ProtoMember(21)]
        [XmlElement(Order = 20, Namespace = UblNamespaces.CommonAggregateComponents2)]
        public List<TransactionConditions> TransactionConditions { get; set; } = new();
    }
}