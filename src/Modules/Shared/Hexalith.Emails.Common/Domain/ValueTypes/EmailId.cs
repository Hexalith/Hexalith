namespace Hexalith.Emails.Domain.ValueTypes
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.Json.Serialization;

    using Hexalith.Domain.ValueTypes;

    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<EmailId>))]
    [JsonConverter(typeof(StringValueJsonConverter<EmailId>))]
    public sealed class EmailId : BusinessId
    {
        public EmailId()
        {
        }

        public EmailId(EmailId emailId) : base(emailId)
        {
        }

        public EmailId(string value) : base(value)
        {
        }

        public static implicit operator EmailId(string value) => new(value);
    }
}