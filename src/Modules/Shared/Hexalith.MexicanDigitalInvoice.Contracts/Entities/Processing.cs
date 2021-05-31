namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Procesamiento", Namespace = MxNamespaces.Fx)]
    [XmlRoot("Procesamiento", Namespace = MxNamespaces.Fx)]
    public class Processing
    {
        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement(Order = 0)]
        public DictionarySet? Dictionary { get; set; } = new();
    }
}