// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 12-19-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="IActorProjectionFactory{TState}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Projections;

using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

/// <summary>
/// Interface IActorProjectionFactory
/// Extends the <see cref="Hexalith.Infrastructure.DaprRuntime.Projections.IActorProjectionFactory" />.
/// </summary>
/// <typeparam name="TState">The type of the t state.</typeparam>
/// <seealso cref="Hexalith.Infrastructure.DaprRuntime.Projections.IActorProjectionFactory" />
public interface IActorProjectionFactory<TState>
{
    /// <summary>
    /// Gets the projection actor.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <returns>IKeyValueActor.</returns>
    IKeyValueActor GetProjectionActor(string aggregateId);

    /// <summary>
    /// Gets the state asynchronous.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;TState&gt;&gt;.</returns>
    Task<TState?> GetStateAsync(string aggregateId, CancellationToken cancellationToken);

    /// <summary>
    /// Sets the state asynchronous.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="state">The state.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SetStateAsync(string aggregateId, TState state, CancellationToken cancellationToken);
}