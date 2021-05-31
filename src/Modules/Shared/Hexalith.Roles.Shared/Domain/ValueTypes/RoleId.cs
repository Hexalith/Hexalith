using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;

using Hexalith.Domain.ValueTypes;

namespace Hexalith.Roles.Domain.ValueTypes
{
    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<RoleId>))]
    [JsonConverter(typeof(StringValueJsonConverter<RoleId>))]
    public sealed class RoleId : BusinessId
    {
        public RoleId()
        {
        }

        public RoleId(RoleId unitId) : base(unitId)
        {
        }

        public RoleId(string value) : base(value)
        {
        }

        public static implicit operator RoleId(string value) => new(value);
    }
}