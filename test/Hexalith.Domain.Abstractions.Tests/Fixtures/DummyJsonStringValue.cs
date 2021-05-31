namespace Hexalith.Domain.Abstractions.Tests.Fixtures
{
    using Hexalith.Domain.ValueTypes;

    public record DummyValue(string AString, string AnullableString, double ADouble, string[] AStringArray);

    public class DummyJsonStringValue : JsonStringValue<DummyValue>
    {
        public DummyJsonStringValue()
        {
        }

        public DummyJsonStringValue(string aString, string aNullableString, double aDouble, string[] aStringArray)
            : base(new DummyValue(aString, aNullableString, aDouble, aStringArray))
        {
        }

        public DummyJsonStringValue(JsonStringValue<DummyValue> value) : base(value)
        {
        }

        public DummyJsonStringValue(DummyValue value) : base(value)
        {
        }

        public DummyJsonStringValue(string value) : base(value)
        {
        }
    }
}