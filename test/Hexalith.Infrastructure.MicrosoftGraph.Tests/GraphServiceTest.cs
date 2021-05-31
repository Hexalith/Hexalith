namespace Hexalith.Infrastructure.MicrosoftGraph.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.MicrosoftGraph.Tests.Fixture;

    using FluentAssertions;

    using Xunit;

    public class GraphServiceTest : IClassFixture<GraphFixture>
    {
        private readonly GraphFixture _graphFixture;

        public GraphServiceTest(GraphFixture graphFixture)
        {
            _graphFixture = graphFixture;
        }

        [Fact]
        public async Task Get_user_ids_should_not_be_empty()
        {
            var userIds = await _graphFixture
                .GraphService
                .GetUserIds();
            userIds.Should().NotBeNull();
            userIds.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Get_user_mails_should_not_be_empty()
        {
            var mails = await _graphFixture
                .GraphService
                .GetUserMails(GraphFixture.GetTestEmail());
            mails.Should().NotBeNull();
            mails.Should().NotBeEmpty();
            mails.First().Attachments.Should().NotBeEmpty();
        }
    }
}