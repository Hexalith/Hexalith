// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.Parties
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="ICustomerProjectionActorFactory.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.PartiesEvents.Helpers;

using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Projections;

/// <summary>
/// Interface ICustomerProjectionActorFactory
/// Extends the <see cref="IActorProjectionFactory" />.
/// </summary>
/// <seealso cref="IActorProjectionFactory" />
public interface ICustomerProjectionActorFactory : IActorProjectionFactory
{
    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;CustomerRegistered&gt;.</returns>
    Task<CustomerRegistered?> GetAsync(string aggregateId, CancellationToken cancellationToken);
}