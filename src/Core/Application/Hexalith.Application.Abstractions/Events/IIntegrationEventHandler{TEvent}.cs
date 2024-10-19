// <copyright file="IIntegrationEventHandler{TEvent}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

/// <summary>
/// Defines a generic interface for handling integration events of a specific type.
/// </summary>
/// <typeparam name="TEvent">The type of the integration event to be handled.</typeparam>
public interface IIntegrationEventHandler<TEvent> : IIntegrationEventHandler
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    where TEvent : class
{
    /// <summary>
    /// Applies the integration event asynchronously.
    /// </summary>
    /// <param name="baseEvent">The integration event to be applied.</param>
    /// <param name="metadata">The metadata associated with the event.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of objects representing the result of applying the event.</returns>
    Task<IEnumerable<object>> ApplyAsync(TEvent baseEvent, Metadata metadata, CancellationToken cancellationToken);
}