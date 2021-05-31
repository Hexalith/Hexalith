using System;
using System.Runtime.Serialization;

namespace Hexalith.Infrastructure.WebServer.Exceptions
{
    [Serializable]
    public class ServerStartupException : Exception
    {
        public ServerStartupException() : this(null)
        {
        }

        public ServerStartupException(string? message) : this(message, null)
        {
        }

        public ServerStartupException(string? message, Exception? innerException)
            : base($"An error occured while starting web server : {message}", innerException)
        {
        }

        protected ServerStartupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}