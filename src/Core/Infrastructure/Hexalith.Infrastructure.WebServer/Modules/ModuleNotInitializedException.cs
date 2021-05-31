using System;
using System.Runtime.Serialization;

namespace Hexalith.Infrastructure.WebServer.Modules
{
    [Serializable]
    internal class ModuleNotInitializedException : Exception
    {
        public ModuleNotInitializedException()
        {
        }

        public ModuleNotInitializedException(string? message) : base(message)
        {
        }

        public ModuleNotInitializedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ModuleNotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}