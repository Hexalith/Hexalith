namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class RepositoryStreamNotFoundException : Exception
    {
        public RepositoryStreamNotFoundException() : this(null)
        {
        }

        public RepositoryStreamNotFoundException(string? message) : this(message, null)
        {
        }

        public RepositoryStreamNotFoundException(string? message, Exception? innerException)
            : this(null, null, message, innerException)
        {
        }

        public RepositoryStreamNotFoundException(object? repository, string? id, string? message = null, Exception? innerException = null)
            : base($"Stream not found. Id='{id}'; Repository:'{repository?.GetType()?.FullName}'.{message}", innerException)
        {
        }

        protected RepositoryStreamNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}