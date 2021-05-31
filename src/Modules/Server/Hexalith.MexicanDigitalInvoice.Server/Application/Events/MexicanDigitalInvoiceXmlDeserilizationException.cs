using System;
using System.Runtime.Serialization;

namespace Hexalith.MexicanDigitalInvoice.Application.Events
{
    [Serializable]
    internal class MexicanDigitalInvoiceXmlDeserilizationException : Exception
    {
        public MexicanDigitalInvoiceXmlDeserilizationException()
        {
        }

        public MexicanDigitalInvoiceXmlDeserilizationException(string? message) : base(message)
        {
        }

        public MexicanDigitalInvoiceXmlDeserilizationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MexicanDigitalInvoiceXmlDeserilizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}