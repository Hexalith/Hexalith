using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;

using Bistrotic.Domain.ValueTypes;

namespace Bistrotic.Users.Domain.ValueTypes
{
    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<UserId>))]
    [JsonConverter(typeof(StringValueJsonConverter<UserId>))]
    public sealed class UserId : BusinessId
    {
        public UserId()
        {
        }

        public UserId(UserId UserId) : base(UserId)
        {
        }

        public UserId(string value) : base(value)
        {
        }

        public static implicit operator UserId(string value) => new UserId(value);
    }
}