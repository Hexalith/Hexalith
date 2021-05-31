namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Traslados", Namespace = MxNamespaces.Cfdi)]
    [XmlRoot("Traslados", Namespace = MxNamespaces.Cfdi)]
    public class TaxTransactions
    {
        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement("Traslado", Order = 0)]
        public List<TaxTransaction> Transaction { get; set; } = new();
    }
}