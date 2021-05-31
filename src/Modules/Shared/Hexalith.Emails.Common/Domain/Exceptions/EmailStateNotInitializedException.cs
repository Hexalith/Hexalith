namespace Hexalith.Emails.Domain.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EmailStateNotInitializedException : Exception
    {
        public EmailStateNotInitializedException()
        {
        }

        public EmailStateNotInitializedException(string? message) : base(message)
        {
        }

        public EmailStateNotInitializedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EmailStateNotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}