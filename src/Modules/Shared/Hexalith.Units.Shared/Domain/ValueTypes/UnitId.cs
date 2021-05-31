using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;

using Hexalith.Domain.ValueTypes;

namespace Hexalith.Units.Domain.ValueTypes
{
    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<UnitId>))]
    [JsonConverter(typeof(StringValueJsonConverter<UnitId>))]
    public sealed class UnitId : BusinessId
    {
        public UnitId()
        {
        }

        public UnitId(UnitId unitId) : base(unitId)
        {
        }

        public UnitId(string value) : base(value)
        {
        }

        public static implicit operator UnitId(string value) => new (value);
    }
}