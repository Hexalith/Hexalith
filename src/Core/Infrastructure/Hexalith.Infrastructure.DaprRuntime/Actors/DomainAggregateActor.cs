// <copyright file="DomainAggregateActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Tasks;
using Hexalith.Applications.Commands;

using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a domain aggregate actor.
/// </summary>
/// <remarks>
/// This actor is responsible for processing commands and events related to a specific domain aggregate.
/// </remarks>
public partial class DomainAggregateActor(
    ActorHost host,
    IDomainCommandDispatcher commandDispatcher,
    IDomainAggregateFactory aggregateFactory,
    TimeProvider dateTimeService,
    IEventBus eventBus,
    IResiliencyPolicyProvider resiliencyPolicyProvider,
    IActorStateManager? actorStateManager = null)
    : DomainAggregateActorBase(host, commandDispatcher, aggregateFactory, dateTimeService, eventBus, resiliencyPolicyProvider, actorStateManager)
{
    /// <summary>
    /// Logs information about processing commands for the actor.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="actorId">The ID of the actor.</param>
    /// <param name="actorType">The type of the actor.</param>
    [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Information,
            Message = "Actor {ActorType} ({ActorId}) is processing commands.")]
    public static partial void LogProcessingCommandsInformation(ILogger logger, string actorId, string actorType);
}