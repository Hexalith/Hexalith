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
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Services;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerInformationChangedHandler.
/// Implements the <see cref="CustomerEventsHandler{CustomerInformationChanged}" />.
/// </summary>
/// <seealso cref="CustomerEventsHandler{CustomerInformationChanged}" />
public partial class CustomerInformationChangedHandler : IntegrationEventHandlerBase<CustomerInformationChanged>
{
    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly IDynamics365FinanceCustomerService _customerService;

    /// <summary>
    /// The logger.
    /// </summary>
#pragma warning disable IDE0052 // Remove unread private members
    private readonly ILogger<CustomerInformationChangedHandler> _logger;
#pragma warning restore IDE0052 // Remove unread private members

    private readonly IOptions<Dynamics365FinancePartiesSettings> _partiesSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerInformationChangedHandler"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="partiesSettings">The parties settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public CustomerInformationChangedHandler(
        IDynamics365FinanceCustomerService customerService,
        IOptions<Dynamics365FinancePartiesSettings> partiesSettings,
        ILogger<CustomerInformationChangedHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(partiesSettings);
        ArgumentNullException.ThrowIfNull(logger);
        _customerService = customerService;
        _partiesSettings = partiesSettings;
        _logger = logger;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(CustomerInformationChanged baseEvent, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        if (_partiesSettings.Value.Customers?.SendCustomersToErpEnabled == true)
        {
            await _customerService.UpdateCustomerAsync(baseEvent, cancellationToken).ConfigureAwait(false);
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