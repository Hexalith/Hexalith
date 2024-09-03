// <copyright file="ActorsCommandProcessor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class ActorsCommandProcessor.
/// Implements the <see cref="ICommandProcessor" />.
/// </summary>
/// <seealso cref="ICommandProcessor" />
[Obsolete("Use AggregateActorCommandProcessor instead.", true)]
public abstract partial class ActorsCommandProcessor : ICommandProcessor
{
    /// <summary>
    /// The actor proxy.
    /// </summary>
    private readonly IActorProxyFactory _actorProxy;

    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorsCommandProcessor"/> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected ActorsCommandProcessor(IActorProxyFactory actorProxy, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(actorProxy);
        ArgumentNullException.ThrowIfNull(logger);
        _actorProxy = actorProxy;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task SubmitAsync(BaseCommand command, BaseMetadata metadata, CancellationToken cancellationToken)
        => await SubmitAsync([command], metadata, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task SubmitAsync(IEnumerable<BaseCommand> commands, BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentNullException.ThrowIfNull(metadata);
        BaseCommand[] commandsArray = commands.ToArray();

        List<string> aggregateNames = commandsArray.Select(p => p.AggregateName).Distinct().ToList();

        foreach (string aggregateName in aggregateNames)
        {
            BaseCommand[] aggregateCommands = commandsArray.Where(p => p.AggregateName == aggregateName).ToArray();
            if (aggregateCommands.Select(p => p.AggregateId).Distinct().Count() > 1)
            {
                throw new InvalidOperationException($"Commands for aggregate '{aggregateName}' must have the same aggregate id.");
            }

            BaseCommand cmd = aggregateCommands[0];
            string actorName = GetActorName(cmd);
            Metadata[] metadatas = aggregateCommands.Select(p => Metadata.CreateNew(p, metadata)).ToArray();

            ActorCommandEnvelope envelope = new(aggregateCommands, metadatas);

            try
            {
                LogSendingCommandsToActor(string.Join(", ", aggregateCommands.Select(p => p.TypeName)), cmd.AggregateId, actorName);
                ICommandProcessorActor actor = _actorProxy.CreateActorProxy<ICommandProcessorActor>(new ActorId(cmd.AggregateId), actorName);
                await actor.DoAsync(envelope).ConfigureAwait(false);
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

    [LoggerMessage(
                EventId = 1,
        Level = LogLevel.Information,
        Message = "Sending commands {CommandNames} to actor {ActorName} for aggregate {AggregateId}.")]
    private partial void LogSendingCommandsToActor(string commandNames, string aggregateId, string actorName);
}