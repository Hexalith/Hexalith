namespace Hexalith.Infrastructure.CodeGeneration.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NamespaceNameNotFoundException : Exception
    {
        public NamespaceNameNotFoundException()
        {
        }

        public NamespaceNameNotFoundException(string? message) : base(message)
        {
        }

        public NamespaceNameNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NamespaceNameNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}