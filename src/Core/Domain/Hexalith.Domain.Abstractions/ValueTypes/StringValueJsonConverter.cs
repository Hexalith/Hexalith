namespace Hexalith.Domain.ValueTypes
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class StringValueJsonConverter<T> : JsonConverter<T>
        where T : SingleValueType<string>, new()
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(T);

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => new() { Value = reader.GetString() ?? string.Empty };

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer?.WriteStringValue(string.IsNullOrEmpty(value?.Value) ? null : value?.Value);
        }
    }
}