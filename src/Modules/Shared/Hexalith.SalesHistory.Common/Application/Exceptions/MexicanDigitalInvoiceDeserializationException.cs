using System;
using System.Runtime.Serialization;

namespace Hexalith.SalesHistory.Application.Exceptions
{
    [Serializable]
    internal class MexicanDigitalInvoiceDeserializationException : Exception
    {
        public MexicanDigitalInvoiceDeserializationException()
        {
        }

        public MexicanDigitalInvoiceDeserializationException(string? message) : base(message)
        {
        }

        public MexicanDigitalInvoiceDeserializationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MexicanDigitalInvoiceDeserializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}