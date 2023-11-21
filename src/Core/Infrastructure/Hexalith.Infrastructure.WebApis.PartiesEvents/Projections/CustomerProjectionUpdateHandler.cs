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
using Hexalith.Application.Projection;
using Hexalith.Application.States;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;

/// <summary>
/// Class CustomerProjectionUpdateHandler.
/// Implements the <see cref="ProjectionUpdateHandler{TCustomerEvent}" />.
/// </summary>
/// <typeparam name="TCustomerEvent">The type of the t customer event.</typeparam>
/// <seealso cref="ProjectionUpdateHandler{TCustomerEvent}" />
public abstract class CustomerProjectionUpdateHandler<TCustomerEvent> : ProjectionUpdateHandler<TCustomerEvent>
    where TCustomerEvent : CustomerEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerProjectionUpdateHandler{TCustomerEvent}" /> class.
    /// </summary>
    /// <param name="stateStoreProvider">The state store provider.</param>
    protected CustomerProjectionUpdateHandler(IStateStoreProvider stateStoreProvider)
        : base(stateStoreProvider)
    {
    }

    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TCustomerEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        Customer customer;
        if (baseEvent is CustomerRegistered registered)
        {
            customer = new Customer(registered);
        }
        else
        {
            Extensions.Common.ConditionalValue<Customer> result = await StateStore.TryGetStateAsync<Customer>(baseEvent.AggregateId, cancellationToken).ConfigureAwait(false);
            customer = result.HasValue
                ? result.Value
                : new Customer(new CustomerRegistered(
                    baseEvent.PartitionId,
                    baseEvent.CompanyId,
                    baseEvent.OriginId,
                    baseEvent.Id,
                    "Not Defined",
                    new Domain.ValueObjets.Contact(null, null, null, null, null),
                    null,
                    null,
                    DateTimeOffset.MinValue));
            customer = (Customer)customer.Apply(baseEvent);
        }

        await StateStore.SetStateAsync(baseEvent.AggregateId, customer, cancellationToken).ConfigureAwait(false);
    }
}