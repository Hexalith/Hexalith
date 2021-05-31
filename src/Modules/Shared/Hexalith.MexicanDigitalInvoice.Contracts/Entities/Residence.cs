namespace Hexalith.MexicanDigitalInvoice.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using ProtoBuf;

    [Serializable]
    [DataContract, ProtoContract]
    [XmlType("Domicilio", Namespace = MxNamespaces.Fx)]
    [XmlRoot("Domicilio", Namespace = MxNamespaces.Fx)]
    public class Residence
    {
        [DataMember(Order = 0), ProtoMember(1)]
        [XmlElement("DomicilioFiscalMexicano", Order = 1)]
        public Address? TaxResidence { get; set; }
    }
}