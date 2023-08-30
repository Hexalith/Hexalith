// ***********************************************************************
// Assembly         : Hexalith.Application.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="IAggregateExternalReferenceQueryService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Application.ExternalSystems.Services;

using Hexalith.Domain.ValueObjets;

/// <summary>
/// Interface IExternalReferenceQueryService.
/// </summary>
public interface IAggregateExternalReferenceQueryService
{
    /// <summary>
    /// Gets the aggregate external references.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;ExternalReference&gt;&gt;.</returns>
    Task<IEnumerable<ExternalReference>> GetAsync(string aggregateId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
    Task<string?> GetExternalIdAsync(string aggregateId, string systemId, CancellationToken cancellationToken);
}