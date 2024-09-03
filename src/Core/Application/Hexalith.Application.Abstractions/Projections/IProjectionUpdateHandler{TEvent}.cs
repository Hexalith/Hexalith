// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="IProjectionUpdateHandler{TEvent}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// ***********************************************************************
namespace Hexalith.Application.Projections;

using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Events;

/// <summary>
/// Interface IProjectionUpdateHandler.
/// </summary>
/// <typeparam name="TEvent">The type of the t event.</typeparam>
public interface IProjectionUpdateHandler<TEvent> : IProjectionUpdateHandler
    where TEvent : IEvent
{
    /// <summary>
    /// Applies the specified event.
    /// </summary>
    /// <param name="baseEvent">The event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task ApplyAsync(TEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Apply as an asynchronous operation.
    /// </summary>
    /// <param name="ev">The event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidCastException">Could not cast event {ev.GetType().Name} to {typeof(TEvent).Name} in projection update handler : {GetType().Name}.</exception>
    async Task IProjectionUpdateHandler.ApplyAsync(IEvent ev, IMetadata metadata, CancellationToken cancellationToken)
    {
        if (ev is TEvent e)
        {
            await ApplyAsync(e, metadata, cancellationToken).ConfigureAwait(false);
            return;
        }

        throw new InvalidCastException($"Could not cast event {ev.GetType().Name} to {typeof(TEvent).Name} in projection update handler : {GetType().Name}");
    }
}