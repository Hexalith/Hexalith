// <copyright file="IIntegrationEventProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Threading.Tasks;

using Hexalith.Domain.Events;

/// <summary>
/// The integration event handler interface.
/// </summary>
public interface IIntegrationEventProcessor
{
    /// <summary>
    /// Submit the event.
    /// </summary>
    /// <param name="baseEvent">The event.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SubmitAsync(IEvent baseEvent, CancellationToken cancellationToken);
}