namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class RepositoryStateDeserializeException : Exception
    {
        public RepositoryStateDeserializeException() : this(null)
        {
        }

        public RepositoryStateDeserializeException(string? message) : this(message, null)
        {
        }

        public RepositoryStateDeserializeException(string? message, Exception? innerException)
            : this(null, null, message, innerException)
        {
        }

        public RepositoryStateDeserializeException(object? repository, string? id, string? message = null, Exception? innerException = null)
            : base($"State can't be deserilized. Id='{id}'; Repository:'{repository?.GetType()?.FullName}'.{message}", innerException)
        {
        }

        protected RepositoryStateDeserializeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}