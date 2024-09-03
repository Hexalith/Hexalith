// <copyright file="IIntegrationEventDispatcher.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using Hexalith.Application.Commands;
using Hexalith.Domain.Events;

/// <summary>
/// Interface IEventDispatcher.
/// </summary>
public interface IIntegrationEventDispatcher
{
    /// <summary>
    /// Applies the asynchronous.
    /// </summary>
    /// <param name="baseEvent">The base event.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;IEnumerable&lt;BaseCommand&gt;&gt;&gt;.</returns>
    Task<IEnumerable<IEnumerable<BaseCommand>>> ApplyAsync(IEvent baseEvent, CancellationToken cancellationToken);
}