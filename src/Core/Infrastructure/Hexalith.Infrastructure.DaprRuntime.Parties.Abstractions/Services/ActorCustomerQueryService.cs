// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Customer
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="ActorCustomerQueryService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Parties.Services;

using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Parties.Commands;
using Hexalith.Application.Parties.Services;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Parties.Actors;

/// <summary>
/// Class CustomerQueryService.
/// Implements the <see cref="ICustomerQueryService" />.
/// </summary>
/// <seealso cref="ICustomerQueryService" />
public class ActorCustomerQueryService : ICustomerQueryService
{
    /// <inheritdoc/>
    public async Task<CustomerInformationChanged?> CreateInformationChangedEventAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateId);
        return await GetActor(aggregateId)
            .CreateInformationChangedEventAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateId);
        return await GetActor(aggregateId)
            .ExistAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> HasChangesAsync(ChangeCustomerInformation change, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(change);
        return await GetActor(change.AggregateId)
            .HasChangesAsync(change)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> IsIntercompanyDirectDeliveryAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateId);
        return await GetActor(aggregateId)
            .IsIntercompanyDirectDeliveryAsync()
            .ConfigureAwait(false);
    }

    private static ICustomerAggregateActor GetActor(string aggregateId)
    {
        return ActorProxy.Create<ICustomerAggregateActor>(
            new ActorId(aggregateId),
            nameof(ICustomerAggregateActor)[1..]);
    }
}