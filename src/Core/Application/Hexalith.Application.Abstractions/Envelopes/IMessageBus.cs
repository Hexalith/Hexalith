// <copyright file="IMessageBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Envelopes;

using Hexalith.Application.Abstractions.Metadatas;

using Hexalith.Domain.Abstractions.Messages;

/// <summary>
/// A message bus is a component that allows to send messages.
/// </summary>
/// <typeparam name="TMessage">The type of the t message.</typeparam>
/// <typeparam name="TMetadata">The type of the t metadata.</typeparam>
public interface IMessageBus<in TMessage, in TMetadata>
    where TMessage : IMessage
    where TMetadata : IMetadata
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
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task PublishAsync(TMessage message, TMetadata metadata, CancellationToken cancellationToken);
}
