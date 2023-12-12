// <copyright file="ExternalReferenceAddedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Server.Dynamics365Finance.Infrastructure.IntegrationEventHandlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Services;

using Microsoft.Extensions.Options;

/// <summary>
/// Class ExternalReferenceAddedHandler.
/// Implements the <see cref="Customers.Infrastructure.IntegrationEventHandlers.CustomerEventsHandler`1" />.
/// </summary>
/// <seealso cref="Customers.Infrastructure.IntegrationEventHandlers.CustomerEventsHandler`1" />
public class ExternalReferenceAddedHandler : IntegrationEventHandler<ExternalSystemReferenceAdded>
{
    /// <summary>
    /// The customer aggregate query service.
    /// </summary>
    private readonly ICustomerAggregateQueryService _customerAggregateQueryService;

    private readonly IDynamics365FinanceClient<CustomerV3> _customerService;
    private readonly IDateTimeService _dateTimeService;
    private readonly string _originId;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalReferenceAddedHandler"/> class.
    /// </summary>
    /// <param name="partnerCustomerService">The partner customer service.</param>
    /// <param name="customerAggregateQueryService">The customer aggregate query service.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ExternalReferenceAddedHandler(
        IDynamics365FinanceClient<CustomerV3> customerService,
        IDynamics365FinanceClient<CustomerExternalSystemCode> partnerCustomerService,
        ICustomerAggregateQueryService customerAggregateQueryService,
        IDateTimeService dateTimeService,
        IOptions<OrganizationSettings> organizationSettings,
        ILogger<ExternalReferenceAddedHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(partnerCustomerService);
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(customerAggregateQueryService);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(organizationSettings);

        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(organizationSettings.Value.DefaultOriginId);
        _customerService = customerService;
        _customerAggregateQueryService = customerAggregateQueryService;
        _dateTimeService = dateTimeService;
        _originId = organizationSettings.Value.DefaultOriginId;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(ExternalSystemReferenceAdded @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return @event.ReferenceAggregateName != Customer.GetAggregateName()
            ? []
            : @event.SystemId == _originId ? [] : await Task.FromResult<IEnumerable<BaseCommand>>([]).ConfigureAwait(false);
    }
}