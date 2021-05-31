namespace Hexalith.Application.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;

    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : class
    {
        public bool CanHandle(Type CommandType)
        {
            return CommandType == typeof(TCommand);
        }

        public abstract Task Handle(Envelope<TCommand> envelope, CancellationToken cancellationToken = default);

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
           => Handle(new Envelope<TCommand>(envelope), cancellationToken);
    }
}