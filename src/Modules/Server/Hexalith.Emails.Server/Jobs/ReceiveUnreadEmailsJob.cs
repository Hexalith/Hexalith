using Hexalith.Emails.Application.Commands;
using Hexalith.Emails.Application.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;

namespace Hexalith.Emails
{
    public sealed class ReceiveUnreadEmailsJob : EmailsJob<ReceiveUnreadEmails>
    {
        public ReceiveUnreadEmailsJob(
            IServiceProvider serviceProvider,
            IOptions<EmailsSettings> settings,
            ILogger<ReceiveUnreadEmailsJob> logger)
            : base(serviceProvider, settings, logger)
        {
        }

        public override int Recurrence => Math.Max(15, Settings.ReceiveUnreadMailsRecurrenceSeconds);

        public override ReceiveUnreadEmails GetCommand(string recipient) => new()
        {
            Recipient = recipient
        };
    }
}