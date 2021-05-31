namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Windows.Input;

    [Serializable]
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException()
        {
        }

        public InvalidCommandException(string aggregateName, ICommand command, string? message, Exception? innerException) :
            base($"The command '{command?.GetType()?.Name ?? "??"}' is invalid for aggregate '{aggregateName ?? "??"}'. {message}", innerException)
        {
        }

        public InvalidCommandException(string? message) : base(message)
        {
        }

        public InvalidCommandException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}