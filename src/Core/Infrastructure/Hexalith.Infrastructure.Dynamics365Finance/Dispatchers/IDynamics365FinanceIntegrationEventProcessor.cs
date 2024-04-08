// <copyright file="IDynamics365FinanceIntegrationEventProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Dispatchers;

using System.Threading.Tasks;

using Hexalith.Application.Events;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;

/// <summary>
/// The integration event handler interface.
/// </summary>
public interface IDynamics365FinanceIntegrationEventProcessor : IIntegrationEventProcessor
{
    /// <summary>
    /// Submit the event.
    /// </summary>
    /// <param name="ievent">The event.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SubmitAsync(Dynamics365BusinessEventBase ievent, CancellationToken cancellationToken);
}