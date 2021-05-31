using System;
using System.Runtime.Serialization;

namespace Hexalith.UblDocuments.Exceptions
{
    [Serializable]
    internal class ReadDateTypeException : Exception
    {

        public ReadDateTypeException()
        {
        }

        public ReadDateTypeException(string? message) : base(message)
        {
        }

        public ReadDateTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ReadDateTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}