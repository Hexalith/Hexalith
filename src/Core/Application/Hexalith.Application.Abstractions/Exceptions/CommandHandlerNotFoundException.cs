namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CommandHandlerNotFoundException : Exception
    {
        public CommandHandlerNotFoundException()
        {
        }

        public CommandHandlerNotFoundException(string? message) : base(message)
        {
        }

        public CommandHandlerNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public CommandHandlerNotFoundException(Type commandType, string? message = null, Exception? innerException = null)
            : base($"The command handler for command '{commandType.Name}' not found. {message}", innerException)
        {
        }

        protected CommandHandlerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}