namespace Hexalith.Infrastructure.WebServer.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class HttpContextNotFoundException : Exception
    {
        public HttpContextNotFoundException() : this(null, null)
        {
        }

        public HttpContextNotFoundException(string? message) : this(message, null)
        {
        }

        public HttpContextNotFoundException(string? message, Exception? innerException) : base("Http context not found." + message, innerException)
        {
        }

        protected HttpContextNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}