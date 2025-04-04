﻿// <copyright file="IMessageBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Envelopes;

using Hexalith.Application.Metadatas;

/// <summary>
/// Defines the contract for a message bus that can publish messages asynchronously.
/// </summary>
public interface IMessageBus
{
    /// <summary>
    /// Publishes a message asynchronously with the specified metadata.
    /// </summary>
    /// <param name="message">The message to be published.</param>
    /// <param name="metadata">The metadata associated with the message.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the publish operation.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    Task PublishAsync(object message, Metadata metadata, CancellationToken cancellationToken);
}