namespace Hexalith.UblDocuments
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    public class EntityBase
    {
        [DataMember(Order = 9000)]
        [XmlElement(Order = 9000)]
        public int Partition { get; set; }
    }
}