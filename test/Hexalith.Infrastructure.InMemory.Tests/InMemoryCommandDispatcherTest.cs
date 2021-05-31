namespace Hexalith.Infrastructure.InMemory.Tests
{
    using Hexalith.Application.Messages;
    using Hexalith.Domain.ValueTypes;
    using Hexalith.Infrastructure.InMemory.Commands;
    using Hexalith.Infrastructure.Tests.Fixture;

    using FluentAssertions;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class InMemoryCommandDispatcherTest
    {
        [Fact]
        public async Task Dispatch_command()
        {
            var commandHandlers = new CommandHandlers();
            var dispatcher = new InMemoryCommandBus(commandHandlers);
            await dispatcher
                .Send(new Envelope<Command1>(new Command1(), new MessageId(), new UserName("User 1"), DateTimeOffset.Now));
            await dispatcher
                .Send(new Envelope<Command2>(new Command2(), new MessageId(), new UserName("User 2"), DateTimeOffset.Now));
            await dispatcher
                .Send(new Envelope<Command3>(new Command3(), new MessageId(), new UserName("User 3"), DateTimeOffset.Now));
            await dispatcher
                .Send(new Envelope<Command4>(new Command4(), new MessageId(), new UserName("User 4"), DateTimeOffset.Now));

            foreach (var pair in commandHandlers)
            {
                pair.Value.Should().NotBeNull();
            }
        }
    }
}