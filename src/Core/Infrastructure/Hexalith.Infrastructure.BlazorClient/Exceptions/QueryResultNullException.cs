using System;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Application.Queries;

namespace Hexalith.Application.Client.Exceptions
{
    [Serializable]
    public class QueryResultNullException : Exception
    {
        public QueryResultNullException()
        {
        }

        public QueryResultNullException(object query, string? message = null, Exception? innerException = null)
            : base($"The query '{query.GetType().Name}' failed. The result object is null.{message}\n{JsonSerializer.Serialize(query)}", innerException)
        {
        }

        public QueryResultNullException(string? message) : base(message)
        {
        }

        public QueryResultNullException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected QueryResultNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}