namespace Hexalith.Application.Commands
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICommandService
    {
        Task Tell<TCommand>(TCommand command, string? messageId = null, CancellationToken cancellationToken = default) where TCommand : class;
    }
}