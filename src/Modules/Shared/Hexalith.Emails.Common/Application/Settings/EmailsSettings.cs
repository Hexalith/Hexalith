using Hexalith.Infrastructure.MicrosoftGraph.Settings;

namespace Hexalith.Emails.Application.Settings
{
    public class EmailsSettings
    {
        public string? ConnectionString { get; init; }
        public MicrosoftGraphAuthenticationSettings? MicrosoftGraph { get; init; }
        public int ReceiveAllMailsRecurrenceSeconds { get; init; }
        public string? ReceiveEmailsRecipient { get; init; }
        public int ReceiveMailsStartSeconds { get; init; }
        public int ReceiveUnreadMailsRecurrenceSeconds { get; init; }
    }
}