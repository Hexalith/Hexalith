namespace Hexalith.MexicanDigitalInvoice
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    using Hexalith.MexicanDigitalInvoice.Aggregates;
    using Hexalith.MexicanDigitalInvoice.Entities;

    using ExtendedXmlSerializer;
    using ExtendedXmlSerializer.Configuration;

    public static class MexicanDigitalInvoiceHelper
    {
        public static IEnumerable<Voucher> GetEmbeddedMexicanDigitalInvoices(this XDocument document)
        {
            List<string> xDocs = document
                .DescendantNodes()
                .Where(p => p.NodeType == XmlNodeType.CDATA
                    && p?
                        .Parent?
                        .Value?
                        .Contains(MxNamespaces.Cfdi,
                            System.StringComparison.InvariantCultureIgnoreCase) == true)
                .Select(p => p?.Parent?.Value?.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .OfType<string>()
                .ToList();
            IExtendedXmlSerializer serializer = new ConfigurationContainer().Create();
            return xDocs.ConvertAll(p => serializer.Deserialize<Voucher>(p));
        }

        public static bool IsMexicanDigitalInvoiceDocument(this string document)
            => !string.IsNullOrWhiteSpace(document)
                && document.Contains(MxNamespaces.Cfdi, System.StringComparison.InvariantCultureIgnoreCase);

        public static XElement RemoveAllNamespaces(this XElement xElement)
        {
            var content = xElement.HasElements ?
                xElement.Elements().Select(RemoveAllNamespaces) :
                (object)xElement.Value;
            return xElement.HasAttributes ?
                new XElement(
                    xElement.Name.LocalName,
                    content,
                    xElement.Attributes().Where(p => !p.IsNamespaceDeclaration)) :
                new XElement(xElement.Name.LocalName, content)
                ;
        }

        public static Voucher ToInvoice(this string document)
        {
            IExtendedXmlSerializer serializer = new ConfigurationContainer()
                .Type<Voucher>()
                .Create();
            return serializer.Deserialize<Voucher>(document);
        }
    }
}