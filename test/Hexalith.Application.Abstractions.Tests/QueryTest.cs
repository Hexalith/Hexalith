namespace Hexalith.Application.Abstractions.Tests
{
    using Hexalith.Application.Abstractions.Tests.Fixture;

    using FluentAssertions;

    using Xunit;

    public class QueryTest
    {
        [Fact]
        public void Query_default_constructor()
        {
            var message = new TestQueryNoId();
            message.MessageId.Should().NotBeNullOrWhiteSpace();
            message.MessageId.Should().HaveLength(22);
            message.Id?.Should().BeNull();
        }

        [Theory]
        [InlineData("L")]
        [InlineData("M@fd1523çççç\\\\èé")]
        [InlineData("AAAAAALLLLLLLLLLLLLLLLLLLLLLLLUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG")]
        [InlineData("        L")]
        [InlineData("L               ")]
        public void QueryId_string_constructor(string value)
        {
            var message = new TestIdQuery(new TestId(value));
            message.MessageId.Should().NotBeNullOrWhiteSpace();
            message.MessageId.Should().HaveLength(22);
            message.Id.Should().Be(value.Trim());
        }
    }
}