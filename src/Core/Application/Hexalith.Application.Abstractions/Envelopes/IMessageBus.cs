// <copyright file="IMessageBus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
    [Obsolete("Use PublishAsync with object message and metadata instead")]
    Task PublishAsync(IEnvelope<TMessage, TMetadata> envelope, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    [Obsolete("Use PublishAsync with object message and metadata instead")]
    Task PublishAsync(TState envelope, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    [Obsolete("Use PublishAsync with object message and metadata instead")]
    Task PublishAsync(TMessage message, TMetadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes a domain message asynchronously.
    /// </summary>
    /// <param name="message">The domain message to publish.</param>
    /// <param name="metadata">The metadata associated with the message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync(object message, MessageMetadatas.Metadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes a domain message asynchronously.
    /// </summary>
    /// <param name="message">The domain message to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync(MessageMetadatas.MessageState message, CancellationToken cancellationToken);
}