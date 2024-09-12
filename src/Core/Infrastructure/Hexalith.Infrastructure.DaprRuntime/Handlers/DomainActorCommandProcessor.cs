// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="DomainActorCommandProcessor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Commands;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class AggregateActorCommandProcessor.
/// Implements the <see cref="ICommandProcessor" />.
/// </summary>
/// <seealso cref="ICommandProcessor" />
public partial class DomainActorCommandProcessor : IDomainCommandProcessor
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
    /// Initializes a new instance of the <see cref="DomainActorCommandProcessor"/> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public DomainActorCommandProcessor(IActorProxyFactory actorProxy, ILogger<DomainActorCommandProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(actorProxy);
        ArgumentNullException.ThrowIfNull(logger);
        _actorProxy = actorProxy;
        _logger = logger;
    }

    /// <summary>
    /// Logs the sending commands to actor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="commandName">Name of the command.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="actorName">Name of the actor.</param>
    [LoggerMessage(
                EventId = 2,
                Level = LogLevel.Information,
                Message = "Sending command {CommandName} to actor {ActorName} for aggregate {AggregateId}.")]
    public static partial void LogSendingCommandToActor(ILogger logger, string commandName, string aggregateId, string actorName);

    /// <inheritdoc/>
    public async Task SubmitAsync(object command, Hexalith.Application.MessageMetadatas.Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        string actorName = DomainAggregateActorBase.GetAggregateActorName(metadata.Message.Aggregate.Name);
        try
        {
            LogSendingCommandToActor(_logger, metadata.Message.Name, metadata.Message.Aggregate.Id, actorName);
            IDomainAggregateActor actor = _actorProxy.CreateActorProxy<IDomainAggregateActor>(new ActorId(metadata.Message.Aggregate.Id), actorName);
            await actor.SubmitCommandAsync(new ActorMessageEnvelope(command, metadata)).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Fail to call actor {actorName} method '{nameof(IAggregateActor.SubmitCommandAsync)}'.", e);
        }
    }
}