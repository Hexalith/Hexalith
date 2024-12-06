// <copyright file="KeyValueActorProjectionUpdateEventHandlerBase{TEvent,TState}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Projections;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

/// <summary>
/// Class KeyValueActorProjectionUpdateEventHandlerBase.
/// Implements the <see cref="IProjectionUpdateHandler{TEvent}" />.
/// </summary>
/// <typeparam name="TEvent">The type of the t event.</typeparam>
/// <typeparam name="TState">The type of the t state.</typeparam>
/// <seealso cref="IProjectionUpdateHandler{TEvent}" />
public abstract class KeyValueProjectionUpdateEventHandlerBase<TEvent, TState> : IProjectionUpdateHandler<TEvent>
    where TEvent : class
    where TState : class
{
    /// <summary>
    /// The actor proxy factory.
    /// </summary>
    private readonly IProjectionFactory<TState> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyValueProjectionUpdateEventHandlerBase{TEvent, TState}" /> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    protected KeyValueProjectionUpdateEventHandlerBase(IProjectionFactory<TState> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <inheritdoc/>
    public abstract Task ApplyAsync(TEvent baseEvent, Metadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the projection asynchronously.
    /// </summary>
    /// <param name="aggregateKey">The key of the aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the projection state.</returns>
    protected virtual async Task<TState?> GetProjectionAsync(string aggregateKey, CancellationToken cancellationToken)
        => await _factory.GetStateAsync(aggregateKey, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Save projection as an asynchronous operation.
    /// </summary>
    /// <param name="aggregatekey">The key of the aggregate.</param>
    /// <param name="state">The state.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected virtual async Task SaveProjectionAsync(string aggregatekey, TState state, CancellationToken cancellationToken)
        => await _factory.SetStateAsync(aggregatekey, state, cancellationToken).ConfigureAwait(false);
}