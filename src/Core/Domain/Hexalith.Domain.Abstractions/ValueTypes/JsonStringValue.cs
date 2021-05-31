namespace Hexalith.Domain.ValueTypes
{
    using System.Diagnostics;
    using System.Text.Json;

    [DebuggerDisplay("{Value}")]
    public abstract class JsonStringValue<T> : SingleValueType<string>
    {
        protected JsonStringValue()
        {
        }

        protected JsonStringValue(JsonStringValue<T> value)
            : base(value)
        {
        }

        protected JsonStringValue(T value)
            : base(JsonSerializer.Serialize(value))
        {
        }

        protected JsonStringValue(string value)
            : base(value)
        {
        }

#pragma warning disable CS8603 // Possible null reference return.
        public static implicit operator T(JsonStringValue<T> value) => (value == null) ? default : JsonSerializer.Deserialize<T>(value.Value);
#pragma warning restore CS8603 // Possible null reference return.
    }
}