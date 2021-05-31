using System;
using System.Runtime.Serialization;

namespace Hexalith.Emails.Domain.Exceptions
{
    [Serializable]
    public class UndefinedEmailIdException : Exception
    {
        public UndefinedEmailIdException()
        {
        }

        public UndefinedEmailIdException(string? message) : base(message)
        {
        }

        public UndefinedEmailIdException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UndefinedEmailIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}