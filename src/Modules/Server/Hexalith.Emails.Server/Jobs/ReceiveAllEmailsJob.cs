using Hexalith.Emails.Application.Commands;
using Hexalith.Emails.Application.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;

namespace Hexalith.Emails
{
    public sealed class ReceiveAllEmailsJob : EmailsJob<ReceiveAllEmails>
    {
        public ReceiveAllEmailsJob(
            IServiceProvider serviceProvider,
            IOptions<EmailsSettings> settings,
            ILogger<ReceiveAllEmailsJob> logger)
            : base(serviceProvider, settings, logger)
        {
        }

        public override int Recurrence => Math.Max(3600, Settings.ReceiveAllMailsRecurrenceSeconds);

        public override ReceiveAllEmails GetCommand(string recipient) => new()
        {
            Recipient = recipient
        };
    }
}