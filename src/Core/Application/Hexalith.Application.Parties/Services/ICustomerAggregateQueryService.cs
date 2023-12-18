// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.PartiesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="ICustomerAggregateQueryService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Parties.Services;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface ICustomerAggregateQueryService.
/// </summary>
public interface ICustomerAggregateQueryService
{
    /// <summary>
    /// Gets the customer asynchronous.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;Customer&gt;&gt;.</returns>
    Task<Customer?> GetCustomerAsync(string aggregateId, CancellationToken cancellationToken);
}