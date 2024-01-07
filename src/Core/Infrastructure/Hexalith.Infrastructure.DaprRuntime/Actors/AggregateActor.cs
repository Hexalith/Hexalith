// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="AggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// Logistics partner catalog item aggregate actor interface <see cref="BspkSalesInvoice" />.
/// Extends the <see cref="IActor" />.
/// </summary>
public partial class AggregateActor : AggregateActorBase, IAggregateActor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateActor"/> class.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <param name="commandDispatcher">The command dispatcher.</param>
    /// <param name="aggregateFactory">The aggregate factory.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="eventBus">The event bus.</param>
    /// <param name="notificationBus">The notification bus.</param>
    /// <param name="commandBus">The command bus.</param>
    /// <param name="requestBus">The request bus.</param>
    /// <param name="resiliencyPolicyProvider">The resiliency policy provider.</param>
    /// <param name="actorStateManager">The actor state manager.</param>
    public AggregateActor(
        ActorHost host,
        ICommandDispatcher commandDispatcher,
        IAggregateFactory aggregateFactory,
        IDateTimeService dateTimeService,
        IEventBus eventBus,
        INotificationBus notificationBus,
        ICommandBus commandBus,
        IRequestBus requestBus,
        IResiliencyPolicyProvider resiliencyPolicyProvider,
        IActorStateManager? actorStateManager = null)
        : base(host, commandDispatcher, aggregateFactory, dateTimeService, eventBus, notificationBus, commandBus, requestBus, resiliencyPolicyProvider, actorStateManager)
    {
    }

    /// <summary>
    /// Logs the processing commands information.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="actorId">The actor identifier.</param>
    /// <param name="actorType">Type of the actor.</param>
    [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Information,
            Message = "Actor {ActorType} ({ActorId}) is processing commands.")]
    public static partial void LogProcessingCommandsInformation(ILogger logger, string actorId, string actorType);
}