namespace Hexalith.WorkItems.Shared.Tests
{
    using Hexalith.WorkItems.Application.Queries;

    using FluentAssertions;

    using Xunit;

    public class GetIssuesWithSlaTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        [InlineData(true, false)]
        public void GetIssuesWithSla_new(bool suspendedSla, bool closedIssues)
        {
            var query = new GetIssuesWithSla(suspendedSla, closedIssues);
            query.SuspendedSla.Should().Be(suspendedSla);
            query.ClosedIssues.Should().Be(closedIssues);
        }

        [Fact]
        public void GetIssuesWithSla_new_should_default_false()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var query = new GetIssuesWithSla();
#pragma warning restore CS0618 // Type or member is obsolete
            query.SuspendedSla.Should().Be(false);
            query.ClosedIssues.Should().Be(false);
        }
    }
}