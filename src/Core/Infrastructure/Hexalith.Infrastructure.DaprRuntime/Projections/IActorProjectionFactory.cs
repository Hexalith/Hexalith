// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 12-19-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="IActorProjectionFactory.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Projections;

using Hexalith.Infrastructure.DaprRuntime.Actors;

/// <summary>
/// Interface IActorProjectionFactory.
/// </summary>
public interface IActorProjectionFactory
{
    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <typeparam name="T">State.</typeparam>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;T&gt;&gt;.</returns>
    Task<T?> GetAsync<T>(string aggregateId, CancellationToken cancellationToken)
        where T : class;

    /// <summary>
    /// Gets the projection actor.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <returns>IKeyValueActor.</returns>
    IKeyValueActor GetProjectionActor(string aggregateId);

    /// <summary>
    /// Sets the asynchronous.
    /// </summary>
    /// <typeparam name="T">State.</typeparam>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="state">The state.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SetAsync<T>(string aggregateId, T state, CancellationToken cancellationToken)
        where T : class;
}