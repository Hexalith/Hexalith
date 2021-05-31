using System;
using System.Runtime.Serialization;

namespace Hexalith.UblDocuments.Application.Events
{
    [Serializable]
    internal class UblXmlDeserilizationException : Exception
    {
        public UblXmlDeserilizationException()
        {
        }

        public UblXmlDeserilizationException(string? message) : base(message)
        {
        }

        public UblXmlDeserilizationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UblXmlDeserilizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}