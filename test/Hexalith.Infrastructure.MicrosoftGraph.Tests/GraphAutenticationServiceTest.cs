namespace Hexalith.Infrastructure.MicrosoftGraph.Tests
{
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.MicrosoftGraph.Tests.Fixture;

    using FluentAssertions;

    using Xunit;

    public class GraphAutenticationServiceTest : IClassFixture<GraphFixture>
    {
        private readonly GraphFixture _graphFixture;

        public GraphAutenticationServiceTest(GraphFixture graphFixture)
        {
            _graphFixture = graphFixture;
        }

        [Fact]
        public async Task Aquire_token()
        {
            var app = _graphFixture
                .AuthenticationService
                .AuthenticationProvider
                .ClientApplication;
            var token = await app
                .AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
                .ExecuteAsync();
            token.Should().NotBeNull();
            token.AccessToken.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void Check_client_initialization()
        {
            _graphFixture
                .AuthenticationService
                .AuthenticationProvider
                .ClientApplication
                .Should()
                .NotBeNull();
        }
    }
}