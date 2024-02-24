// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Projections
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="ExternalReferenceMapperService.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Helpers;

/// <summary>
/// Class AggregateExternalReferenceQueryService.
/// Implements the <see cref="IExternalReferenceMapperService" />.
/// </summary>
/// <seealso cref="IExternalReferenceMapperService" />
public class ExternalReferenceMapperService : IExternalReferenceMapperService
{
    private readonly string _applicationName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalReferenceMapperService"/> class.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    public ExternalReferenceMapperService(string applicationName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        _applicationName = applicationName;
    }

    /// <inheritdoc/>
    public async Task<string?> GetAggregateIdAsync(string aggregateName, string partitionId, string companyId, string systemId, string externalId, CancellationToken cancellationToken)
    {
        return await GetExternalReferenceToAggregateActor(aggregateName, partitionId, companyId, systemId, externalId)
            .GetAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<string?> GetExternalIdAsync(string aggregateName, string aggregateId, string systemId, CancellationToken cancellationToken)
    {
        return await GetAggregateToExternalReferenceActor(aggregateName, aggregateId, systemId)
            .GetAsync()
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the aggregate external reference actor.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <returns>IAggregateExternalReferenceProjectionActor.</returns>
    private IKeyValueActor GetAggregateToExternalReferenceActor(string aggregateName, string aggregateId, string systemId)
    {
        return ActorProxy.Create<IKeyValueActor>(
            new ActorId(ExternalSystemsProjectionsHelper.CreateExternalReferenceMapperId(aggregateId, systemId)),
            ExternalSystemsProjectionsHelper.GetAggregateToExternalReferenceActorName(_applicationName, aggregateName));
    }

    /// <summary>
    /// Gets the external reference to aggregate actor.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <returns>IExternalReferenceToAggregateActor.</returns>
    private IKeyValueActor GetExternalReferenceToAggregateActor(string aggregateName, string partitionId, string companyId, string systemId, string externalId)
    {
        return ActorProxy.Create<IKeyValueActor>(
            new ActorId(ExternalSystemsProjectionsHelper.CreateExternalReferenceMapperId(partitionId, companyId, systemId, externalId)),
            ExternalSystemsProjectionsHelper.GetAggregateToExternalReferenceActorName(_applicationName, aggregateName));
    }
}