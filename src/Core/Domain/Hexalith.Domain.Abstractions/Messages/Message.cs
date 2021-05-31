namespace Hexalith.Domain.Messages
{
    using System.Text.Json.Serialization;

    using Hexalith.Domain.ValueTypes;

    public abstract class Message : IMessage
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string? Id => null;

        public string MessageId { get; } = new MessageId();
    }
}