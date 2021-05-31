namespace Hexalith.Application.Commands
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;

    /// <summary>
    /// Interface ICommandHandler
    /// </summary>
    public interface ICommandHandler
    {
        Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface ICommandHandler
    /// </summary>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    public interface ICommandHandler<TCommand> : ICommandHandler
        where TCommand : class
    {
        Task Handle(Envelope<TCommand> envelope, CancellationToken cancellationToken = default);
    }
}