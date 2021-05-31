namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BusinessObjectNotFoundException : Exception
    {
        public BusinessObjectNotFoundException() : base()
        {
        }

        public BusinessObjectNotFoundException(string? message) : base(message)
        {
        }

        public BusinessObjectNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public BusinessObjectNotFoundException(string? name, string? id, string? message = null, Exception? innerException = null) : base(message, innerException)
        {
            Name = name;
            Id = id;
        }

        protected BusinessObjectNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public string? Id { get; }
        public string? Name { get; }
    }
}