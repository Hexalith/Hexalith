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
/// <remarks>
/// Initializes a new instance of the <see cref="CustomerProjectionUpdateHandler{TCustomerEvent}"/> class.
/// </remarks>
/// <param name="factory">The factory.</param>
/// <param name="logger">The logger.</param>
public abstract partial class CustomerProjectionUpdateHandler<TCustomerEvent>(IActorProjectionFactory<Customer> factory, ILogger logger)
    : KeyValueActorProjectionUpdateEventHandlerBase<TCustomerEvent, Customer>(factory)
    where TCustomerEvent : CustomerEvent
{
    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TCustomerEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);

        if (baseEvent is CustomerRegistered registered)
        {
            await SaveProjectionAsync(baseEvent.AggregateId, new Customer(registered), cancellationToken).ConfigureAwait(false);
            return;
        }

        Customer? existingCustomer = await GetProjectionAsync(baseEvent.AggregateId, cancellationToken).ConfigureAwait(false);
        if (existingCustomer == null)
        {
            if (baseEvent is CustomerInformationChanged changed)
            {
                await SaveProjectionAsync(
                        baseEvent.AggregateId,
                        changed.ToCustomer(false),
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            CustomerProjectionUpdateHandler<TCustomerEvent>.LogCustomerProjectionNotInitialized(logger, baseEvent.AggregateId, baseEvent.TypeName);
            return;
        }

        (IAggregate? aggregate, _) = existingCustomer.Apply(baseEvent);
        await SaveProjectionAsync(baseEvent.AggregateId, (Customer)aggregate, cancellationToken).ConfigureAwait(false);
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Warning,
        Message = "Customer projection not initialized for aggregate {AggregateId} and event {TypeName}.")]
    private static partial void LogCustomerProjectionNotInitialized(ILogger logger, string aggregateId, string typeName);
}