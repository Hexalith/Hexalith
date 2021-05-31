using System;
using System.Runtime.Serialization;

namespace Hexalith.MexicanDigitalInvoice.Infrastructure
{
    [Serializable]
    internal class SetTypeNotSupportedException<TSet> : Exception
    {
        public SetTypeNotSupportedException() : this(null)
        {
        }

        public SetTypeNotSupportedException(string? message) : this(message, null)
        {
        }

        public SetTypeNotSupportedException(object? repository, string? message, Exception? innerException)
            : base($"Set of type {typeof(TSet).Name} does not exist in repository {repository?.GetType().Name}. {message}", innerException)
        {
        }

        public SetTypeNotSupportedException(object repository)
            : this(repository, null, null)
        {
        }

        public SetTypeNotSupportedException(string? message, Exception? innerException)
             : this(null, message, innerException)
        {
        }

        protected SetTypeNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}