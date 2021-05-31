namespace Hexalith.Infrastructure.Tests.Fixture
{
    using Hexalith.Application.Commands;
    using Hexalith.Application.Messages;
    using Hexalith.Domain.ValueTypes;

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITestCommandHandler : ICommandHandler
    {
        int CallCount { get; }
    }

    public sealed class Business1Id : BusinessId
    {
        public Business1Id(string value) : base(value)
        {
        }

        public Business1Id(BusinessId value) : base(value)
        {
        }

        public Business1Id() : base()
        {
        }
    }

    public sealed class Business2Id : BusinessId
    {
        public Business2Id(string value) : base(value)
        {
        }

        public Business2Id(BusinessId value) : base(value)
        {
        }

        public Business2Id() : base()
        {
        }
    }

    public sealed class Command1 : TestCommand<Business1Id>
    {
        public Command1()
        {
        }

        public Command1(Business1Id id) : base(id)
        {
        }
    }

    public sealed class Command2 : TestCommand<Business2Id>
    {
        public Command2()
        {
        }

        public Command2(Business2Id id) : base(id)
        {
        }
    }

    public sealed class Command3 : TestCommand
    {
    }

    public sealed class Command4 : TestCommand
    {
    }

    public class CommandHandler1 : TestCommandHandler<Command1>
    {
    }

    public class CommandHandler2 : TestCommandHandler<Command2>
    {
    }

    public class CommandHandler3 : TestCommandHandler<Command3>
    {
    }

    public class CommandHandler4 : TestCommandHandler<Command4>
    {
    }

    public class CommandHandlers : Dictionary<Type, Func<ICommandHandler>>
    {
        public CommandHandlers()
        {
            Add(typeof(Command1), () => new CommandHandler1());
            Add(typeof(Command2), () => new CommandHandler2());
            Add(typeof(Command3), () => new CommandHandler3());
            Add(typeof(Command4), () => new CommandHandler4());
        }
    }

    public abstract class TestCommand<TId>
        where TId : BusinessId, new()
    {
        protected TestCommand()
        {
            Id = new TId();
        }

        protected TestCommand(TId id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public abstract class TestCommand
    {
    }

    public class TestCommandHandler<TCommand> : CommandHandler<TCommand>, ITestCommandHandler
        where TCommand : class
    {
        public int CallCount { get; private set; }

        public override Task Handle(Envelope<TCommand> envelope, CancellationToken cancellationToken = default)
        {
            CallCount++;
            return Task.CompletedTask;
        }
    }
}