namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class RepositoryOutboxMessageDeserializeException : Exception
    {
        public RepositoryOutboxMessageDeserializeException() : this(null)
        {
        }

        public RepositoryOutboxMessageDeserializeException(string? message) : this(message, null)
        {
        }

        public RepositoryOutboxMessageDeserializeException(string? message, Exception? innerException)
            : this(null, null, message, innerException)
        {
        }

        public RepositoryOutboxMessageDeserializeException(object? repository, string? messageId, string? message, Exception? innerException = null)
            : base($"Outbox message can't be deserilized. Id='{messageId}'; Repository:'{repository?.GetType()?.FullName}'.{message}", innerException)
        {
        }

        protected RepositoryOutboxMessageDeserializeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}