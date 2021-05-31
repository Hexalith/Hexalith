namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class RepositoryStateNotFoundException : Exception
    {
        public RepositoryStateNotFoundException() : this(null)
        {
        }

        public RepositoryStateNotFoundException(string? message) : this(message, null)
        {
        }

        public RepositoryStateNotFoundException(string? message, Exception? innerException)
            : this(null, null, message, innerException)
        {
        }

        public RepositoryStateNotFoundException(object? repository, string? id, string? message = null, Exception? innerException = null)
            : base($"State not found. Id='{id}'; Repository:'{repository?.GetType()?.FullName}'.{message}", innerException)
        {
        }

        protected RepositoryStateNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}