namespace Hexalith.Emails.Domain.States
{
    using System.Collections.Generic;

    using Hexalith.Emails.Contracts.Events;
    using Hexalith.Emails.Contracts.ValueTypes;

    public class EmailState : IEmailState
    {
        public List<Attachment> Attachments { get; set; } = new();

        public string Body { get; set; } = string.Empty;
        public List<string> CopyToRecipients { get; set; } = new();
        public string Recipient { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public List<string> ToRecipients { get; set; } = new();

        public void Apply(EmailReceived received)
        {
            Subject = received.Subject;
            Body = received.Body;
            Sender = received.Sender;
            ToRecipients = new List<string>(received.ToRecipients);
            CopyToRecipients = new List<string>(received.CopyToRecipients);
            Recipient = received.Recipient;
            Attachments = new List<Attachment>(received.Attachments);
        }
    }
}