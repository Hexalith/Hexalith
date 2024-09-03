// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 12-19-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="KeyValueActorProjectionUpdateEventHandlerBase{TEvent,TState}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Projections;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Domain.Events;

/// <summary>
/// Class KeyValueActorProjectionUpdateEventHandlerBase.
/// Implements the <see cref="IProjectionUpdateHandler{TEvent}" />.
/// </summary>
/// <typeparam name="TEvent">The type of the t event.</typeparam>
/// <typeparam name="TState">The type of the t state.</typeparam>
/// <seealso cref="IProjectionUpdateHandler{TEvent}" />
public abstract class KeyValueActorProjectionUpdateEventHandlerBase<TEvent, TState> : IProjectionUpdateHandler<TEvent>
    where TEvent : IEvent
    where TState : class
{
    /// <summary>
    /// The actor proxy factory.
    /// </summary>
    private readonly IActorProjectionFactory<TState> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyValueActorProjectionUpdateEventHandlerBase{TEvent, TState}" /> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected KeyValueActorProjectionUpdateEventHandlerBase(IActorProjectionFactory<TState> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <inheritdoc/>
    public abstract Task ApplyAsync(TEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Get projection as an asynchronous operation.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;TState&gt; representing the asynchronous operation.</returns>
    protected virtual async Task<TState?> GetProjectionAsync(string aggregateId, CancellationToken cancellationToken)
            => await _factory.GetStateAsync(aggregateId, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Save projection as an asynchronous operation.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="state">The state.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected virtual async Task SaveProjectionAsync(string aggregateId, TState state, CancellationToken cancellationToken)
        => await _factory.SetStateAsync(aggregateId, state, cancellationToken).ConfigureAwait(false);
}