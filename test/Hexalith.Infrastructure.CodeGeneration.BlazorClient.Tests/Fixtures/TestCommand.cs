namespace Hexalith.Infrastructure.CodeGeneration.Tests.Fixtures
{
    using Hexalith.Application.Commands;
    using Hexalith.Domain.Contracts.Commands;
    using Hexalith.Domain.ValueTypes;

    [Command]
    public class TestCommand
    {
        public string Id { get; }
        public string MessageId { get; } = new MessageId();
    }
}