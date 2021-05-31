namespace Hexalith.Emails.Domain.States
{
    using System.Collections.Generic;

    using Hexalith.Emails.Contracts.Events;
    using Hexalith.Emails.Contracts.ValueTypes;

    public interface IEmailState
    {
        List<Attachment> Attachments { get; set; }
        string Body { get; set; }
        List<string> CopyToRecipients { get; set; }
        string Recipient { get; set; }
        string Sender { get; set; }
        string Subject { get; set; }
        List<string> ToRecipients { get; set; }

        void Apply(EmailReceived received);
    }
}