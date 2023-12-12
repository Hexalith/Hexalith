// <copyright file="CustomerInformationChangedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Server.Dynamics365Finance.Infrastructure.IntegrationEventHandlers;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;

/// <summary>
/// Class CustomerInformationChangedHandler.
/// Implements the <see cref="CustomerEventsHandler{CustomerInformationChanged}" />.
/// </summary>
/// <seealso cref="CustomerEventsHandler{CustomerInformationChanged}" />
public class CustomerInformationChangedHandler : IntegrationEventHandler<CustomerInformationChanged>
{
    private readonly IDynamics365FinanceClient<CustomerBase> _customerBaseService;
    private readonly IDynamics365FinanceClient<CustomerV3> _customerService;
    private readonly ILogger<CustomerInformationChanged> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerInformationChangedHandler"/> class.
    /// </summary>
    /// <param name="customerBaseService">The customer base service.</param>
    /// <param name="customerService">The customer service.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public CustomerInformationChangedHandler(
        IDynamics365FinanceClient<CustomerBase> customerBaseService,
        IDynamics365FinanceClient<CustomerV3> customerService,
        ILogger<CustomerInformationChanged> logger)
    {
        ArgumentNullException.ThrowIfNull(customerBaseService);
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(logger);
        _customerBaseService = customerBaseService;
        _customerService = customerService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(CustomerInformationChanged @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        CustomerAccountKey customerKey = new(@event.CompanyId, @event.Id);
        DateTimeOffset? birthDate = @event.Contact?.Person?.BirthDate;
        CustomerV3 customer = await _customerService.GetSingleAsync(customerKey, cancellationToken).ConfigureAwait(false);
        Dictionary<string, object?> delta = customer.GetChanges(@event);
        if (delta.Count > 1)
        {
            await _customerService
                    .PatchAsync(customerKey, delta, cancellationToken).ConfigureAwait(false);
        }

        CustomerBase customerBase = await _customerBaseService.GetSingleAsync(customerKey, cancellationToken).ConfigureAwait(false);
        delta = customerBase.GetChanges(@event);
        if (delta.Count > 1)
        {
            await _customerBaseService
                    .PatchAsync(customerKey, delta, cancellationToken).ConfigureAwait(false);
        }

        return [];
    }
}