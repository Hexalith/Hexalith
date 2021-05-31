namespace Hexalith.Domain.ValueTypes
{
    using System;
    using System.ComponentModel;
    using System.Text.Json;

    public class JsonStringValueConverter<T> : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
                => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
                => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return JsonSerializer.Deserialize<T>(stringValue);
#pragma warning restore CS8603 // Possible null reference return.
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is T t)
            {
                return JsonSerializer.Serialize(t);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}