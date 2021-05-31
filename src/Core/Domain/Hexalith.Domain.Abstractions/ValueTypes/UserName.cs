namespace Hexalith.Domain.ValueTypes
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text.Json.Serialization;

    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<UserName>))]
    [JsonConverter(typeof(StringValueJsonConverter<UserName>))]
    public sealed class UserName : StringValue
    {
        public UserName()
        {
            Value = string.Empty;
        }

        public UserName(string? value)
        {
            Value = value?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(value) || Value.Length > DomainConstants.MaxUserNameLength)
            {
                throw new ArgumentException($"User name not defined or has more than {DomainConstants.MaxUserNameLength} characters. Name : '{Value}'.", nameof(value));
            }
        }

        public static implicit operator UserName(string? value) => value == null ? new UserName() : new UserName(value);
    }
}