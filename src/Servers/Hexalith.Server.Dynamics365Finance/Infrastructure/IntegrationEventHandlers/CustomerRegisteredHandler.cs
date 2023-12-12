// <copyright file="CustomerInformationChangedHandler_.cs" company="Fiveforty SAS Paris France">
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
/// Class CustomerRegisteredHandler.
/// Implements the <see cref="CustomerEventsHandler{CustomerRegistered}" />.
/// </summary>
/// <seealso cref="CustomerEventsHandler{CustomerRegistered}" />
public class CustomerRegisteredHandler : IntegrationEventHandler<CustomerRegistered>
{
    private readonly IDynamics365FinanceClient<CustomerBase> _customerBaseService;
    private readonly IDynamics365FinanceClient<CustomerV3> _customerService;
    private readonly ILogger<CustomerRegistered> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRegisteredHandler"/> class.
    /// </summary>
    /// <param name="customerBaseService">The customer base service.</param>
    /// <param name="customerService">The customer service.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public CustomerRegisteredHandler(
        IDynamics365FinanceClient<CustomerBase> customerBaseService,
        IDynamics365FinanceClient<CustomerV3> customerService,
        ILogger<CustomerRegistered> logger)
    {
        ArgumentNullException.ThrowIfNull(customerBaseService);
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(logger);
        _customerBaseService = customerBaseService;
        _customerService = customerService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(CustomerRegistered @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        CustomerAccountKey customerKey = new(@event.CompanyId, @event.Id);
        DateTimeOffset? birthDate = @event.Contact?.Person?.BirthDate;
        await _customerService
                .PatchAsync(customerKey, @event.ToDynamics365FinanceCustomer(), cancellationToken).ConfigureAwait(false);
        _ = _customerBaseService.PatchAsync(
            customerKey,
            new CustomerBase(
                @event.CompanyId,
                @event.Id,
                null,
                @event.Contact?.Person?.Title,
                birthDate?.Day,
                birthDate?.Month,
                birthDate?.Year),
            cancellationToken);
        return [];
    }
}