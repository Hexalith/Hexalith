namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class QueryHandlerNotFoundException : Exception
    {
        public QueryHandlerNotFoundException()
        {
        }

        public QueryHandlerNotFoundException(string? message) : base(message)
        {
        }

        public QueryHandlerNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public QueryHandlerNotFoundException(Type queryType, string? message = null, Exception? innerException = null)
            : base($"The query handler for query '{queryType.Name}' not found. {message}", innerException)
        {
        }

        protected QueryHandlerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}