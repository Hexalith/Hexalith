// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-04-2023
// ***********************************************************************
// <copyright file="SalesInvoiceAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using System;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Domain.Aggregates;

using Hexalith.Extensions.Common;

/// <summary>
/// Logistics partner catalog item aggregate actor interface <see cref="BspkSalesInvoice" />.
/// Extends the <see cref="IActor" />.
/// </summary>
public class SalesInvoiceAggregateActor : AggregateActor, ISalesInvoiceAggregateActor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceAggregateActor"/> class.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <param name="commandDispatcher">The command dispatcher.</param>
    /// <param name="aggregateFactory">The aggregate factory.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="eventBus">The event bus.</param>
    /// <param name="notificationBus">The notification bus.</param>
    /// <param name="commandBus">The command bus.</param>
    /// <param name="requestBus">The request bus.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public SalesInvoiceAggregateActor(
        ActorHost host,
        ICommandDispatcher commandDispatcher,
        IAggregateFactory aggregateFactory,
        IDateTimeService dateTimeService,
        IEventBus eventBus,
        INotificationBus notificationBus,
        ICommandBus commandBus,
        IRequestBus requestBus)
        : base(host, commandDispatcher, aggregateFactory, dateTimeService, eventBus, notificationBus, commandBus, requestBus)
        => ArgumentNullException.ThrowIfNull(host);

    /// <inheritdoc/>
    public async Task<string> SayHelloAsync() => await Task.FromResult("Say hello").ConfigureAwait(false);
}