namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class RepositoryStateNullException : Exception
    {
        public RepositoryStateNullException() : this(null)
        {
        }

        public RepositoryStateNullException(string? message) : this(message, null)
        {
        }

        public RepositoryStateNullException(string? message, Exception? innerException)
            : this(null, null, message, innerException)
        {
        }

        public RepositoryStateNullException(object? repository, string? id, string? message = null, Exception? innerException = null)
            : base($"Id='{id}'; Repository:'{repository?.GetType()?.FullName}'.{message}", innerException)
        {
        }

        protected RepositoryStateNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}