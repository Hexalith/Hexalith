// <copyright file="AggregateActor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Application.Tasks;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprRuntime.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// The aggregate manager actor class.
/// Implements the <see cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// Implements the <see cref="IAggregateActor" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// <seealso cref="IAggregateActor" />
public partial class AggregateActor(
    ActorHost host,
    ICommandDispatcher commandDispatcher,
    IAggregateFactory aggregateFactory,
    TimeProvider dateTimeService,
    IEventBus eventBus,
    INotificationBus notificationBus,
    ICommandBus commandBus,
    IRequestBus requestBus,
    IResiliencyPolicyProvider resiliencyPolicyProvider,
    IActorStateManager? actorStateManager = null)
    : AggregateActorBase(host, commandDispatcher, aggregateFactory, dateTimeService, eventBus, notificationBus, commandBus, requestBus, resiliencyPolicyProvider, actorStateManager)
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