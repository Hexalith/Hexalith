// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 11-18-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-13-2023
// ***********************************************************************
// <copyright file="CustomerRegisteredHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;

using System.Threading;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Services;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerRegisteredHandler.
/// Implements the <see cref="CustomerEventsHandler{CustomerRegistered}" />.
/// </summary>
/// <seealso cref="CustomerEventsHandler{CustomerRegistered}" />
public partial class CustomerRegisteredHandler : IntegrationEventHandlerBase<CustomerRegistered>
{
    private readonly IDynamics365FinanceCustomerService _customerService;

    /// <summary>
    /// The logger.
    /// </summary>
#pragma warning disable IDE0052 // Remove unread private members
    private readonly ILogger<CustomerRegisteredHandler> _logger;
#pragma warning restore IDE0052 // Remove unread private members

    /// <summary>
    /// The organization settings.
    /// </summary>
    private readonly IOptions<OrganizationSettings> _organizationSettings;

    /// <summary>
    /// The origin identifier.
    /// </summary>
    private readonly string? _originId;

    private readonly IOptions<Dynamics365FinancePartiesSettings> _partiesSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRegisteredHandler"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="partiesSettings">The parties settings.</param>
    /// <param name="organizationSettings">The organization settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public CustomerRegisteredHandler(
        IDynamics365FinanceCustomerService customerService,
        IOptions<Dynamics365FinancePartiesSettings> partiesSettings,
        IOptions<OrganizationSettings> organizationSettings,
        ILogger<CustomerRegisteredHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(partiesSettings);
        ArgumentNullException.ThrowIfNull(organizationSettings);
        ArgumentNullException.ThrowIfNull(logger);

        ArgumentException.ThrowIfNullOrWhiteSpace(organizationSettings.Value.DefaultOriginId);

        _customerService = customerService;
        _partiesSettings = partiesSettings;
        _organizationSettings = organizationSettings;
        _logger = logger;
        _originId = _organizationSettings.Value.DefaultOriginId;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(CustomerRegistered baseEvent, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        if (baseEvent.OriginId.Equals(_originId, StringComparison.OrdinalIgnoreCase))
        {
            return [];
        }

        if (_partiesSettings.Value.Customers?.SendCustomersToErpEnabled == true)
        {
            _ = await _customerService.CreateCustomerAsync(baseEvent, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            LogSendCustomersToErpIsDisabledInformation();
        }

        return [];
    }

    [LoggerMessage(
    EventId = 1,
    Level = LogLevel.Information,
    Message = "Send customers to Dynamics 365 Finance is disabled.")]
    private partial void LogSendCustomersToErpIsDisabledInformation();
}