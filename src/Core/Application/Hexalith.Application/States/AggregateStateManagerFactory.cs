// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 04-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-25-2023
// ***********************************************************************
// <copyright file="AggregateStateManagerFactory.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.States;

using System;

using Hexalith.Application.Abstractions.Aggregates;
using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Events;
using Hexalith.Application.Abstractions.Notifications;
using Hexalith.Domain.Abstractions.Aggregates;
using Hexalith.Extensions.Common;

/// <summary>
/// Class AggregateStateManagerFactory.
/// Implements the <see cref="IAggregateStateManagerFactory" />.
/// </summary>
/// <seealso cref="IAggregateStateManagerFactory" />
public class AggregateStateManagerFactory : IAggregateStateManagerFactory
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// The dispatcher.
    /// </summary>
    private readonly ICommandDispatcher _dispatcher;

    /// <summary>
    /// The event bus.
    /// </summary>
    private readonly IEventBus _eventBus;

    /// <summary>
    /// The notification bus.
    /// </summary>
    private readonly INotificationBus _notificationBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateStateManagerFactory"/> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="eventBus">The event bus.</param>
    /// <param name="notificationBus">The notification bus.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <exception cref="System.ArgumentNullException">Null values.</exception>
    public AggregateStateManagerFactory(
    ICommandDispatcher dispatcher,
    IEventBus eventBus,
    INotificationBus notificationBus,
    IDateTimeService dateTimeService)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(eventBus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(notificationBus);
        _dateTimeService = dateTimeService;
        _dispatcher = dispatcher;
        _eventBus = eventBus;
        _notificationBus = notificationBus;
    }

    /// <inheritdoc/>
    public IAggregateStateManager<TAggregate> CreateManager<TAggregate>()
        where TAggregate : IAggregate => new AggregateStateManager<TAggregate>(_dispatcher, _eventBus, _notificationBus, _dateTimeService);
}