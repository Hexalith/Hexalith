namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    using Hexalith.Application.Repositories;

    [Serializable]
    internal class MissingEventsException<TRepository> : Exception
        where TRepository : IRepository
    {
        public MissingEventsException()
            : this(null)
        {
        }

        public MissingEventsException(string? message)
            : this(message, null)
        {
        }

        public MissingEventsException(string id, string? message, Exception? innerException)
            : base($"There are no events to save. Repository='{typeof(TRepository).Name}'; Id='{id}'.\n{message}", innerException)
        {
        }

        public MissingEventsException(string? message, Exception? innerException)
            : this(string.Empty, message, innerException)
        {
        }

        protected MissingEventsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}