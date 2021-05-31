using System;
using System.Runtime.Serialization;

using Hexalith.Emails.Application.Settings;

namespace Hexalith.Emails.Exceptions
{
    [Serializable]
    internal class MailboxRecipientNotDefinedException : Exception
    {
        public MailboxRecipientNotDefinedException() : this(null)
        {
        }

        public MailboxRecipientNotDefinedException(string? message) : this(message, null)
        {
        }

        public MailboxRecipientNotDefinedException(string? message, Exception? innerException)
            : base($"The mailbox recipient name not defined. Set a value for setting {nameof(EmailsSettings) + ":" + nameof(EmailsSettings.ReceiveEmailsRecipient)}.\n{message}", innerException)
        {
        }

        protected MailboxRecipientNotDefinedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}