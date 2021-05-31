namespace Hexalith.Domain.ValueTypes
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.Json.Serialization;

    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<MessageId>))]
    [JsonConverter(typeof(StringValueJsonConverter<MessageId>))]
    public sealed class MessageId : AutoIdentifier
    {
        public MessageId(MessageId messageId)
            : base(messageId)
        {
        }

        public MessageId()
        {
        }

        public MessageId(string id)
            : base(id)
        {
        }

        public static implicit operator MessageId(string value) => new (value);
    }
}