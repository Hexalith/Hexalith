namespace Hexalith.Domain.Abstractions.Tests
{
    using Hexalith.Domain.Abstractions.Tests.Fixtures;

    using FluentAssertions;

    using Xunit;

    public class JsonStringValueTest
    {
        [Fact]
        public void JsonStringValue_check_values()
        {
            var value = new DummyJsonStringValue("hello", "hi", 15.5, new[] { "one", "two" });
            DummyValue v = value;
            v.AString.Should().Be("hello");
            v.AnullableString.Should().Be("hi");
            v.ADouble.Should().Be(15.5);
            v.AStringArray.Should().BeEquivalentTo(new[] { "one", "two" });
        }
    }
}