namespace Hexalith.Domain.ValueTypes
{
    using System.ComponentModel;
    using System.Diagnostics;

    [DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(StringValueConverter<Etag>))]
    public sealed class Etag : AutoIdentifier
    {
        public Etag()
        {
        }

        public Etag(Etag etag)
            : base(etag)
        {
        }

        public Etag(string etag)
            : base(etag)
        {
        }

        public static implicit operator Etag(string value) => new(value);
    }
}