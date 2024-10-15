// <copyright file="IIntegrationEventDispatcher.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using Hexalith.Application.MessageMetadatas;

/// <summary>
/// Defines the contract for an integration event dispatcher.
/// </summary>
public interface IIntegrationEventDispatcher
{
    /// <summary>
    /// Applies the integration event asynchronously.
    /// </summary>
    /// <param name="baseEvent">The base event to be applied.</param>
    /// <param name="metadata">The metadata associated with the event.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of collections of objects representing the results of applying the event.</returns>
    Task<IEnumerable<IEnumerable<object>>> ApplyAsync(object baseEvent, Metadata metadata, CancellationToken cancellationToken);
}