namespace Hexalith.UblDocuments.Types.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlRoot(Namespace = UblNamespaces.CommonAggregateComponents2)]
    [XmlType(Namespace = UblNamespaces.CommonAggregateComponents2)]
    public class PricingReference
    {
        [DataMember(Order = 1), ProtoMember(2)]
        [XmlElement(Order = 1, Namespace = UblNamespaces.CommonBasicComponents2)]
        public List<Price> AlternativeConditionPrice { get; set; } = new();

        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0, Namespace = UblNamespaces.CommonBasicComponents2)]
        public ItemLocationQuantity? OriginalItemLocationQuantity { get; set; }
    }
}