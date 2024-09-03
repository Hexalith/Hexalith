// <copyright file="IDomainMessageBus.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Envelopes;

using Hexalith.Application.Metadatas;

/// <summary>
/// Represents a domain message bus.
/// </summary>
public interface IDomainMessageBus
{
    /// <summary>
    /// Publishes a domain message asynchronously.
    /// </summary>
    /// <param name="message">The domain message to publish.</param>
    /// <param name="metadata">The metadata associated with the message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync(object message, Metadata metadata, CancellationToken cancellationToken);
}