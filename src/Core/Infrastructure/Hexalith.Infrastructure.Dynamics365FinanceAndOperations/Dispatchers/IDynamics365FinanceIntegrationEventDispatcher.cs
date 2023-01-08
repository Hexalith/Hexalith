// <copyright file="IDynamics365FinanceIntegrationEventDispatcher.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Dispatchers;

using Hexalith.Application.Abstractions.Events;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;

using System.Threading.Tasks;

/// <summary>
/// The integration event handler interface.
/// </summary>
public interface IDynamics365FinanceIntegrationEventDispatcher : IIntegrationEventDispatcher
{
    /// <summary>
    /// Dispatch the event.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task DispatchAsync(Dynamics365BusinessEventBase @event, CancellationToken cancellationToken);
}
