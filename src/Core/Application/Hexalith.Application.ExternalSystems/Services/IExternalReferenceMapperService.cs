// ***********************************************************************
// Assembly         : Hexalith.Application.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="IExternalReferenceMapperService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Application.ExternalSystems.Services;

/// <summary>
/// Interface IExternalReferenceQueryService.
/// </summary>
public interface IExternalReferenceMapperService
{
    /// <summary>
    /// Gets the aggregate identifier asynchronous.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
    Task<string?> GetAggregateIdAsync(string aggregateName, string partitionId, string companyId, string systemId, string externalId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the external identifier asynchronous.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
    Task<string?> GetExternalIdAsync(string aggregateName, string aggregateId, string systemId, CancellationToken cancellationToken);
}