// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="AggregateActorCommandProcessor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class AggregateActorCommandProcessor.
/// Implements the <see cref="ICommandProcessor" />.
/// </summary>
/// <seealso cref="ICommandProcessor" />
public partial class AggregateActorCommandProcessor : ICommandProcessor
{
    /// <summary>
    /// The actor proxy.
    /// </summary>
    private readonly IActorProxyFactory _actorProxy;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateActorCommandProcessor" /> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public AggregateActorCommandProcessor(IActorProxyFactory actorProxy, ILogger<AggregateActorCommandProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(actorProxy);
        ArgumentNullException.ThrowIfNull(logger);
        _actorProxy = actorProxy;
        _logger = logger;
    }

    /// <inheritdoc/>
    [Obsolete]
    public async Task SubmitAsync(BaseCommand command, BaseMetadata metadata, CancellationToken cancellationToken)
        => await SubmitAsync([command], metadata, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    [Obsolete]
    public async Task SubmitAsync(IEnumerable<BaseCommand> commands, BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentNullException.ThrowIfNull(metadata);
        BaseCommand[] commandsArray = commands.ToArray();

        List<string> aggregateNames = commandsArray.Select(p => p.AggregateName).Distinct().ToList();

        // Group commands by aggregate name
        foreach (string aggregateName in aggregateNames)
        {
            BaseCommand[] aggregateCommands = commandsArray.Where(p => p.AggregateName == aggregateName).ToArray();

            // Group commands by aggregate id
            List<string> aggregateIds = aggregateCommands.Select(p => p.AggregateId).Distinct().ToList();

            string actorName = AggregateActorBase.GetAggregateActorName(aggregateName);

            foreach (string aggregateId in aggregateIds)
            {
                BaseCommand[] aggregateIdCommands = aggregateCommands.Where(p => p.AggregateId == aggregateId).ToArray();
                BaseCommand cmd = aggregateIdCommands[0];
                Metadata[] metadatas = aggregateIdCommands.Select(p => Metadata.CreateNew(p, metadata)).ToArray();

                ActorCommandEnvelope envelope = new(aggregateIdCommands, metadatas);

                try
                {
                    LogSendingCommandsToActor(string.Join(", ", aggregateIdCommands.Select(p => p.TypeName)), cmd.AggregateId, actorName);
                    IAggregateActor actor = _actorProxy.CreateActorProxy<IAggregateActor>(new ActorId(cmd.AggregateId), actorName);
                    await actor.SubmitCommandAsync(envelope).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException($"Fail to call actor {actorName} method '{nameof(IAggregateActor.SubmitCommandAsync)}'.", e);
                }
            }
        }
    }

    /// <inheritdoc/>
    [Obsolete]
    public async Task SubmitAsync(object command, Hexalith.Application.MessageMetadatas.Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        string actorName = AggregateActorBase.GetAggregateActorName(metadata.Message.Aggregate.Name);
        try
        {
            LogSendingCommandsToActor(metadata.Message.Name, metadata.Message.Aggregate.Id, actorName);
            IAggregateActor actor = _actorProxy.CreateActorProxy<IAggregateActor>(new ActorId(metadata.Message.Aggregate.Id), actorName);
            await actor.SubmitCommandAsync(new ActorMessageEnvelope(command, metadata)).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Fail to call actor {actorName} method '{nameof(IAggregateActor.SubmitCommandAsync)}'.", e);
        }
    }

    /// <summary>
    /// Logs the sending commands to actor.
    /// </summary>
    /// <param name="commandNames">The command names.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="actorName">Name of the actor.</param>
    [LoggerMessage(
                EventId = 1,
        Level = LogLevel.Information,
        Message = "Sending commands {CommandNames} to actor {ActorName} for aggregate {AggregateId}.")]
    private partial void LogSendingCommandsToActor(string commandNames, string aggregateId, string actorName);

    /// <summary>
    /// Logs the sending commands to actor.
    /// </summary>
    /// <param name="commandName"></param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="actorName">Name of the actor.</param>
    [LoggerMessage(
                EventId = 2,
        Level = LogLevel.Information,
        Message = "Sending command {CommandName} to actor {ActorName} for aggregate {AggregateId}.")]
    private partial void LogSendingCommandToActor(string commandName, string aggregateId, string actorName);
}