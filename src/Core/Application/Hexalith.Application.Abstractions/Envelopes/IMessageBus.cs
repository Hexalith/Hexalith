// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-15-2023
// ***********************************************************************
// <copyright file="IMessageBus.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Envelopes;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Messages;

/// <summary>
/// A message bus is a component that allows to send messages.
/// </summary>
/// <typeparam name="TMessage">The type of the t message.</typeparam>
/// <typeparam name="TMetadata">The type of the t metadata.</typeparam>
/// <typeparam name="TState">The type of the t state.</typeparam>
[Obsolete("Use IDomainMessageBus instead", false)]
public interface IMessageBus<in TMessage, in TMetadata, in TState>
    where TMessage : BaseMessage
    where TMetadata : BaseMetadata
    where TState : MessageState<TMessage, TMetadata>
{
    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task PublishAsync(IEnvelope<TMessage, TMetadata> envelope, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task PublishAsync(TState envelope, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task PublishAsync(TMessage message, TMetadata metadata, CancellationToken cancellationToken);
}