// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.PartiesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="CustomerProjectionUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.PartiesEvents.Projections;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Projections;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class CustomerProjectionUpdateHandler.
/// Implements the <see cref="ProjectionUpdateHandler{TCustomerEvent}" />.
/// </summary>
/// <typeparam name="TCustomerEvent">The type of the t customer event.</typeparam>
/// <seealso cref="ProjectionUpdateHandler{TCustomerEvent}" />
public abstract partial class CustomerProjectionUpdateHandler<TCustomerEvent> : KeyValueActorProjectionUpdateEventHandlerBase<TCustomerEvent, CustomerRegistered>
    where TCustomerEvent : CustomerEvent
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerProjectionUpdateHandler{TCustomerEvent}"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="logger">The logger.</param>
    protected CustomerProjectionUpdateHandler(IActorProjectionFactory<CustomerRegistered> factory, ILogger logger)
        : base(factory) => _logger = logger;

    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TCustomerEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        if (baseEvent is CustomerRegistered registered)
        {
            await SaveProjectionAsync(baseEvent.AggregateId, registered, cancellationToken).ConfigureAwait(false);
            return;
        }

        Customer customer;
        CustomerRegistered? existingCustomer = await GetProjectionAsync(baseEvent.AggregateId, cancellationToken).ConfigureAwait(false);
        if (existingCustomer == null)
        {
            if (baseEvent is CustomerInformationChanged changed)
            {
                await SaveProjectionAsync(baseEvent.AggregateId, changed.ToCustomer(false).ToCustomerRegistered(), cancellationToken).ConfigureAwait(false);
            }

            LogCustomerProjectionNotInitialized(baseEvent.AggregateId, baseEvent.TypeName);
            return;
        }

        customer = new Customer(existingCustomer);
        (IAggregate? aggregate, _) = customer.Apply(baseEvent);
        customer = (Customer)aggregate;
        await SaveProjectionAsync(baseEvent.AggregateId, customer.ToCustomerRegistered(), cancellationToken).ConfigureAwait(false);
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Warning,
        Message = "Customer projection not initialized for aggregate {AggregateId} and event {TypeName}.")]
    private partial void LogCustomerProjectionNotInitialized(string aggregateId, string typeName);
}