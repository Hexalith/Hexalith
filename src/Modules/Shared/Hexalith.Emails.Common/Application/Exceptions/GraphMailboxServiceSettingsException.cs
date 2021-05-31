using System;
using System.Runtime.Serialization;

namespace Hexalith.Emails.Application.Exceptions
{
    [Serializable]
    internal class GraphMailboxServiceSettingsException : Exception
    {
        public GraphMailboxServiceSettingsException() : this(null)
        {
        }

        public GraphMailboxServiceSettingsException(string? message) : this(string.Empty, string.Empty, message, null)
        {
        }

        public GraphMailboxServiceSettingsException(string settingName, string settingValue, string? message = default, Exception? innerException = default)
            : base($"Incorrect settings value '{settingValue}' for {settingName}.\n{message}", innerException)
        {
        }

        public GraphMailboxServiceSettingsException(string? message, Exception? innerException)
            : this(string.Empty, string.Empty, message, innerException)
        {
        }

        protected GraphMailboxServiceSettingsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}