namespace Hexalith.Emails.Domain.ValueTypes
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.Json.Serialization;

    using Hexalith.Domain.ValueTypes;

    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<EmailAddress>))]
    [JsonConverter(typeof(StringValueJsonConverter<EmailAddress>))]
    public sealed class EmailAddress : StringValue
    {
        public EmailAddress()
        {
        }

        public EmailAddress(EmailAddress emailId) : base(emailId)
        {
        }

        public EmailAddress(string value) : base(value)
        {
        }

        public static implicit operator EmailAddress(string value) => new(value);
    }
}