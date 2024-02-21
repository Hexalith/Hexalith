// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Projections
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="ExternalReferenceMapperUpdateService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Services;

using System.Threading.Tasks;

using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Helpers;

/// <summary>
/// Class AggregateExternalReferenceQueryService.
/// Implements the <see cref="IExternalReferenceMapperUpdateService" />.
/// </summary>
/// <seealso cref="IExternalReferenceMapperUpdateService" />
public class ExternalReferenceMapperUpdateService
{
    /// <summary>
    /// The aggregate name.
    /// </summary>
    private readonly string _aggregateName;

    private readonly string _applicationName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalReferenceMapperUpdateService"/> class.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    public ExternalReferenceMapperUpdateService(string applicationName, string aggregateName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateName);
        _applicationName = applicationName;
        _aggregateName = aggregateName;
    }

    /// <summary>
    /// Set aggregate identifier as an asynchronous operation.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task SetAggregateIdAsync(string partitionId, string companyId, string systemId, string externalId, string? aggregateId)
    {
        await ExternalSystemsProjectionsHelper
            .GetExternalReferenceToAggregateActor(_applicationName, _aggregateName, partitionId, companyId, systemId, externalId)
            .SetAsync(aggregateId)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Set external identifier as an asynchronous operation.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task SetExternalIdAsync(string aggregateId, string systemId, string? externalId)
    {
        await ExternalSystemsProjectionsHelper
            .GetAggregateToExternalReferenceActor(_applicationName, _aggregateName, aggregateId, systemId)
            .SetAsync(externalId)
            .ConfigureAwait(false);
    }
}