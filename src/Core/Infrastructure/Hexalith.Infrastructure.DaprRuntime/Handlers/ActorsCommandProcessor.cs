// <copyright file="ActorsCommandProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Client;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

/// <summary>
/// Class ActorsCommandProcessor.
/// Implements the <see cref="ICommandProcessor" />.
/// </summary>
/// <seealso cref="ICommandProcessor" />
public abstract class ActorsCommandProcessor : ICommandProcessor
{
    /// <summary>
    /// The actor proxy.
    /// </summary>
    private readonly IActorProxyFactory _actorProxy;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorsCommandProcessor" /> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    public ActorsCommandProcessor(IActorProxyFactory actorProxy)
    {
        ArgumentNullException.ThrowIfNull(actorProxy);
        _actorProxy = actorProxy;
    }

    /// <inheritdoc/>
    public async Task SubmitAsync(BaseCommand command, BaseMetadata metadata, CancellationToken cancellationToken)
        => await SubmitAsync(command.IntoArray(), metadata, cancellationToken);

    /// <inheritdoc/>
    public async Task SubmitAsync(IEnumerable<BaseCommand> commands, BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(commands);

        string? aggregateName = null;
        List<List<BaseCommand>> lists = new();
        List<BaseCommand> aggregateCommands = new();
        foreach (BaseCommand cmd in commands)
        {
            if (aggregateName != cmd.AggregateName)
            {
                aggregateName = cmd.AggregateName;
                if (aggregateCommands.Count > 0)
                {
                    lists.Add(aggregateCommands);
                }

                aggregateCommands = new List<BaseCommand>();
            }

            aggregateCommands.Add(cmd);
        }

        if (aggregateCommands.Count > 0)
        {
            lists.Add(aggregateCommands);
        }

        foreach (List<BaseCommand> list in lists)
        {
            BaseCommand cmd = list.First();
            string actorName = GetActorName(cmd);
            List<BaseMetadata> metadatas = list.Select(p => Metadata.CreateNew(p, metadata)).ToList<BaseMetadata>();

            ActorCommandEnvelope envelope = new(list.ToArray(), metadatas.ToArray());

            try
            {
                ICommandProcessorActor actor = _actorProxy.CreateActorProxy<ICommandProcessorActor>(cmd.AggregateId.ToUrlEncodedActorId(), actorName);
                await actor.DoAsync(envelope);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Fail to call actor {actorName} method '{nameof(ICommandProcessorActor.DoAsync)}'.", e);
            }
        }
    }

    /// <summary>
    /// Gets the name of the actor method.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>string.</returns>
    protected abstract string GetActorMethodName(ICommand command);

    /// <summary>
    /// Gets the name of the actor.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>string.</returns>
    protected abstract string GetActorName(ICommand command);
}