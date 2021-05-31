using Hexalith.Application.Messages;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hexalith.Application.Commands
{
    public interface ICommandBus
    {
        Task Send<TCommand>(Envelope<TCommand> envelope, CancellationToken cancellationToken = default)
            where TCommand : class;

        Task Send(IEnvelope envelope, CancellationToken cancellationToken = default);
        Task Send(IEnumerable<IEnvelope> list, CancellationToken cancellationToken = default);
    }
}