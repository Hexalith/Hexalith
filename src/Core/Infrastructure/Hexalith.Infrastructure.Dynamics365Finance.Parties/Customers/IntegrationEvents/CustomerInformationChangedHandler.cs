// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 11-18-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-13-2023
// ***********************************************************************
// <copyright file="CustomerInformationChangedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Services;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class CustomerInformationChangedHandler.
/// Implements the <see cref="CustomerEventsHandler{CustomerInformationChanged}" />.
/// </summary>
/// <seealso cref="CustomerEventsHandler{CustomerInformationChanged}" />
public class CustomerInformationChangedHandler : IntegrationEventHandler<CustomerInformationChanged>
{
    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly IDynamics365FinanceCustomerService _customerService;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<CustomerInformationChangedHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerInformationChangedHandler" /> class.
    /// </summary>
    /// <param name="customerBaseService">The customer base service.</param>
    /// <param name="customerService">The customer service.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    public CustomerInformationChangedHandler(
        IDynamics365FinanceCustomerService customerService,
        ILogger<CustomerInformationChangedHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(logger);
        _customerService = customerService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(CustomerInformationChanged @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        await _customerService.UpdateCustomerAsync(@event, cancellationToken).ConfigureAwait(false);

        return [];
    }
}