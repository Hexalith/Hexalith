namespace Hexalith.DataIntegrationTypes.Domain.ValueTypes
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.Json.Serialization;

    using Hexalith.Domain.ValueTypes;

    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<DataIntegrationTypeId>))]
    [JsonConverter(typeof(StringValueJsonConverter<DataIntegrationTypeId>))]
    internal sealed class DataIntegrationTypeId : BusinessId
    {
        public DataIntegrationTypeId()
        {
        }

        public DataIntegrationTypeId(DataIntegrationTypeId dataIntegrationTypeId) : base(dataIntegrationTypeId)
        {
        }

        public DataIntegrationTypeId(string value) : base(value)
        {
        }

        public DataIntegrationTypeId(string name, string documentName) : base($"{name}-{documentName}")
        {
        }

        public static implicit operator DataIntegrationTypeId(string value) => new(value);
    }
}