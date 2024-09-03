// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-05-2023
// ***********************************************************************
// <copyright file="IEventBus.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Events;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Events;

/// <summary>
/// A event bus is a component that allows to send events.
/// </summary>
public interface IEventBus : IMessageBus<BaseEvent, BaseMetadata, EventState>
{
    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    new Task PublishAsync(IEnvelope<BaseEvent, BaseMetadata> envelope, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    new Task PublishAsync(EventState state, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="baseEvent">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    new Task PublishAsync(BaseEvent baseEvent, BaseMetadata metadata, CancellationToken cancellationToken);
}