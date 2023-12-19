// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.PartiesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="CustomerAggregateQueryService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.PartiesEvents.Services;

using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Hexalith.Application.Parties.Services;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Helpers;

/// <summary>
/// Class CustomerAggregateQueryService.
/// </summary>
public class CustomerAggregateQueryService : ICustomerProjectionService
{
    /// <summary>
    /// The state store.
    /// </summary>
    private readonly ICustomerProjectionActorFactory _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerAggregateQueryService"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public CustomerAggregateQueryService(ICustomerProjectionActorFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <summary>
    /// Get customer as an asynchronous operation.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Customer&gt; representing the asynchronous operation.</returns>
    public async Task<Customer?> GetCustomerAsync([NotNull] string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(aggregateId);

        CustomerRegistered? result = await _factory
            .GetAsync(aggregateId, cancellationToken)
            .ConfigureAwait(false);
        return (result == null) ? null : new Customer(result);
    }
}