namespace Hexalith.Infrastructure.Abstractions.Tests
{
    using Hexalith.Infrastructure.Helpers;

    using FluentAssertions;

    using Xunit;

    public class CryptographicTest
    {
        [Theory]
        [InlineData("L l")]
        [InlineData("/L l/")]
        [InlineData("\\/L l\\/")]
        [InlineData("àaA")]
        [InlineData("Eéèe")]
        [InlineData("M@fd1523çççç\\\\èé/+ {}")]
        [InlineData("+-*/=")]
        public void ToMD5Base64_should_return_22_chars(string value)
        {
            var hash = value.ToMD5Base64();
            hash.Should().NotBeNullOrWhiteSpace();
            hash.Should().HaveLength(22);
        }

        [Theory]
        [InlineData("L l")]
        [InlineData("/L l/")]
        [InlineData("\\/L l\\/")]
        [InlineData("àaA")]
        [InlineData("Eéèe")]
        [InlineData("M@fd1523çççç\\\\èé/+ {}")]
        [InlineData("+-*/=")]
        public void ToSHA256Base64_should_return_43_chars(string value)
        {
            var hash = value.ToSha256Base64();
            hash.Should().NotBeNullOrWhiteSpace();
            hash.Should().HaveLength(43);
        }

        [Theory]
        [InlineData("L l")]
        [InlineData("/L l/")]
        [InlineData("\\/L l\\/")]
        [InlineData("àaA")]
        [InlineData("Eéèe")]
        [InlineData("M@fd1523çççç\\\\èé/+ {}")]
        [InlineData("+-*/=")]
        public void ToSHA512Base64_should_return_86_chars(string value)
        {
            var hash = value.ToSha512Base64();
            hash.Should().NotBeNullOrWhiteSpace();
            hash.Should().HaveLength(86);
        }
    }
}