// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="AggregateExternalReferenceQueryService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Actors;

/// <summary>
/// Class AggregateExternalReferenceQueryService.
/// Implements the <see cref="IAggregateExternalReferenceQueryService" />.
/// </summary>
/// <seealso cref="IAggregateExternalReferenceQueryService" />
public class AggregateExternalReferenceQueryService : IAggregateExternalReferenceQueryService
{
    /// <inheritdoc/>
    public async Task<IEnumerable<ExternalReference>> GetAsync(string aggregateId, CancellationToken cancellationToken)
    {
        return await GetActor(aggregateId)
            .GetExternalReferencesAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<string?> GetExternalIdAsync(string aggregateId, string systemId, CancellationToken cancellationToken)
    {
        return await GetActor(aggregateId)
            .GetSystemReferenceAsync(systemId)
            .ConfigureAwait(false);
    }

    private static IAggregateExternalReferenceAggregateActor GetActor(string aggregateId)
    {
        return ActorProxy.Create<IAggregateExternalReferenceAggregateActor>(
            new ActorId(aggregateId),
            nameof(IAggregateExternalReferenceAggregateActor)[1..]);
    }
}