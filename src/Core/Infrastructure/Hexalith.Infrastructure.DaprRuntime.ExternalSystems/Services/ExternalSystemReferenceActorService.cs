// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="ExternalSystemReferenceActorService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Services;

using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Actors;

/// <summary>
/// Class ExternalSystemReferenceQueryService.
/// Implements the <see cref="IExternalSystemReferenceQueryService" />.
/// </summary>
/// <seealso cref="IExternalSystemReferenceQueryService" />
public class ExternalSystemReferenceActorService : IExternalSystemReferenceQueryService
{
    /// <inheritdoc/>
    public async Task<string?> GetAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(aggregateId);
        return await GetActor(aggregateId)
            .GetReferenceAggregateIdAsync()
            .ConfigureAwait(false);
    }

    private static IExternalSystemReferenceAggregateActor GetActor(string aggregateId)
    {
        return ActorProxy.Create<IExternalSystemReferenceAggregateActor>(
            new ActorId(aggregateId),
            nameof(ExternalSystemReferenceAggregateActor));
    }
}