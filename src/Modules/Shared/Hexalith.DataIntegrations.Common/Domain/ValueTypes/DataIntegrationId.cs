namespace Hexalith.DataIntegrations.Domain.ValueTypes
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.Json.Serialization;

    using Hexalith.Domain.ValueTypes;

    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<DataIntegrationId>))]
    [JsonConverter(typeof(StringValueJsonConverter<DataIntegrationId>))]
    internal sealed class DataIntegrationId : BusinessId
    {
        public DataIntegrationId()
        {
        }

        public DataIntegrationId(DataIntegrationId dataIntegrationId) : base(dataIntegrationId)
        {
        }

        public DataIntegrationId(string value) : base(value)
        {
        }

        public DataIntegrationId(string name, string documentName) : base($"{name}-{documentName}")
        {
        }

        public static implicit operator DataIntegrationId(string value) => new(value);
    }
}