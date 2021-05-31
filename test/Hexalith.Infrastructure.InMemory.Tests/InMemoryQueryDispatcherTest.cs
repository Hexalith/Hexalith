namespace Hexalith.Infrastructure.InMemory.Tests
{
    using System;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;
    using Hexalith.Domain.ValueTypes;
    using Hexalith.Infrastructure.InMemory.Queries;
    using Hexalith.Infrastructure.Tests.Fixture;

    using FluentAssertions;

    using Xunit;

    public class InMemoryQueryDispatcherTest
    {
        [Fact]
        public async Task Dispatch_query_check_return_values()
        {
            var dispatcher = new InMemoryQueryBus(new QueryHandlers());
            (await dispatcher.Dispatch<Query1, int>(
                    new Envelope<Query1>(new Query1(), new MessageId(), new UserName("User 1"), DateTimeOffset.Now)
                    ).ConfigureAwait(false))
                .Should().Be(1);
            (await dispatcher.Dispatch<Query2, string>(
                    new Envelope<Query2>(new Query2(), new MessageId(), new UserName("User 2"), DateTimeOffset.Now)
                    ).ConfigureAwait(false))
                .Should().Be("2");
            (await dispatcher.Dispatch<Query3, Guid>(new Envelope<Query3>(new Query3(), new MessageId(), new UserName("User 3"), DateTimeOffset.Now)
                ).ConfigureAwait(false))
                .Should().Be(new Guid("66CABB1C-18E3-4E26-AE0F-EA603D9F11FB"));
            var query = new Envelope<Query4>(new Query4(), new MessageId(), new UserName("User 4"), DateTimeOffset.Now);
            (await dispatcher.Dispatch<Query4, string>(query).ConfigureAwait(false))
                .Should().Be(query.Message.MessageId);
        }
    }
}