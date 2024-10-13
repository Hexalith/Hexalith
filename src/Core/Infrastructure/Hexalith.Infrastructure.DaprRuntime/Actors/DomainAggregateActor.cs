// <copyright file="DomainAggregateActor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System.Text.Json;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Application.Tasks;
using Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// The aggregate manager actor class.
/// Implements the <see cref="AggregateActorBase" />
/// Implements the <see cref="IAggregateActor" />.
/// </summary>
/// <seealso cref="AggregateActorBase" />
/// <seealso cref="IAggregateActor" />
public partial class DomainAggregateActor(
    ActorHost host,
    IDomainCommandDispatcher commandDispatcher,
    IDomainAggregateFactory aggregateFactory,
    TimeProvider dateTimeService,
    IEventBus eventBus,
    INotificationBus notificationBus,
    ICommandBus commandBus,
    IRequestBus requestBus,
    IResiliencyPolicyProvider resiliencyPolicyProvider,
    JsonSerializerOptions jsonOptions,
    IActorStateManager? actorStateManager = null)
    : DomainAggregateActorBase(host, commandDispatcher, aggregateFactory, dateTimeService, eventBus, notificationBus, commandBus, requestBus, resiliencyPolicyProvider, jsonOptions, actorStateManager)
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