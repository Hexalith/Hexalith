// <copyright file="CustomerInformationChangedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Server.Dynamics365Finance.Infrastructure.IntegrationEventHandlers;

using Bspk.Customers.Infrastructure.IntegrationEventHandlers;

using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

/// <summary>
/// Class CustomerInformationChangedHandler.
/// Implements the <see cref="CustomerEventsHandler{CustomerInformationChanged}" />.
/// </summary>
/// <seealso cref="CustomerEventsHandler{CustomerInformationChanged}" />
public class CustomerInformationChangedHandler : CustomerEventsHandler<CustomerInformationChanged>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerInformationChangedHandler" /> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="logger">The logger.</param>
    public CustomerInformationChangedHandler(IDynamics365FinanceClient<CustomerV3> customerService, ILogger<CustomerInformationChangedHandler> logger)
        : base(customerService, logger)
    {
    }
}