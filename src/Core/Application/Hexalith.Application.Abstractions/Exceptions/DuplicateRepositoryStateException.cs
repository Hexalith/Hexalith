namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DuplicateRepositoryStateException : Exception
    {
        public DuplicateRepositoryStateException() : this(null)
        {
        }

        public DuplicateRepositoryStateException(string? message) : this(message, null)
        {
        }

        public DuplicateRepositoryStateException(string? message, Exception? innerException)
            : this(null, null, message, innerException)
        {
        }

        public DuplicateRepositoryStateException(object? repository, string? id, string? message = null, Exception? innerException = null)
            : base($"Duplicate state key. Id='{id}'; Repository:'{repository?.GetType()?.FullName}'.{message}", innerException)
        {
        }

        protected DuplicateRepositoryStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}