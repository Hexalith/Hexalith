// <copyright file="IIntegrationEventHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Commons.Metadatas;

/// <summary>
/// Defines the contract for handling integration events in the application.
/// </summary>
/// <remarks>
/// This interface is used to implement handlers for integration events,
/// which are events that can affect multiple aggregates or bounded contexts.
/// </remarks>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

public interface IIntegrationEventHandler
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    /// <summary>
    /// Applies the integration event asynchronously.
    /// </summary>
    /// <param name="baseEvent">The integration event to be applied.</param>
    /// <param name="metadata">Metadata associated with the event.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// an enumerable of objects representing the results of applying the event.
    /// </returns>
    Task<IEnumerable<object>> ApplyAsync(object baseEvent, Metadata metadata, CancellationToken cancellationToken);
}