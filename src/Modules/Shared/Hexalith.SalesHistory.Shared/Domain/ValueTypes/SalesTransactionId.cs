namespace Bistrotic.SalesHistory.Domain.ValueTypes
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.Json.Serialization;

    using Bistrotic.Domain.ValueTypes;

    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<SalesTransactionId>))]
    [JsonConverter(typeof(StringValueJsonConverter<SalesTransactionId>))]
    public sealed class SalesTransactionId : BusinessId
    {
        public SalesTransactionId()
        {
        }

        public SalesTransactionId(SalesTransactionId unitId) : base(unitId)
        {
        }

        public SalesTransactionId(string value) : base(value)
        {
        }

        public static implicit operator SalesTransactionId(string value) => new(value);
    }
}