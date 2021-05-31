namespace Hexalith.Domain.ValueTypes
{
    using System;
    using System.ComponentModel;

    public class StringValueConverter<T> : TypeConverter
     where T : StringValue, new()
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
                => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
                => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return new T { Value = stringValue };
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is T t)
            {
                return t.Value;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}