// <copyright file="IProjectionUpdateHandler{TEvent}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Projections;

using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

/// <summary>
/// Interface IProjectionUpdateHandler.
/// </summary>
/// <typeparam name="TEvent">The type of the t event.</typeparam>
public interface IProjectionUpdateHandler<in TEvent> : IProjectionUpdateHandler
    where TEvent : class
{
    /// <summary>
    /// Applies the specified event.
    /// </summary>
    /// <param name="baseEvent">The event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task ApplyAsync(TEvent baseEvent, Metadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Apply as an asynchronous operation.
    /// </summary>
    /// <param name="ev">The event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidCastException">Could not cast event {ev.GetType().Name} to {typeof(TEvent).Name} in projection update handler : {GetType().Name}.</exception>
#pragma warning disable S4039 // Interface methods should be callable by derived types

    async Task IProjectionUpdateHandler.ApplyAsync(object ev, Metadata metadata, CancellationToken cancellationToken)
#pragma warning restore S4039 // Interface methods should be callable by derived types
    {
        if (ev is TEvent e)
        {
            await ApplyAsync(e, metadata, cancellationToken).ConfigureAwait(false);
            return;
        }

        throw new InvalidCastException($"Could not cast event {ev.GetType().Name} to {typeof(TEvent).Name} in projection update handler : {GetType().Name}");
    }
}