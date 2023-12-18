// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerRegisteredHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using Hexalith.Application.Organizations.Configurations;
using Hexalith.Application.Parties.Services;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerEventHandler.
/// Implements the <see cref="Application.Events.IntegrationEventHandler{Bspk.Customers.Infrastructure.IntegrationEvents.FFYCustomerRegisteredBusinessEvent}" />.
/// </summary>
/// <seealso cref="Application.Events.IntegrationEventHandler{Bspk.Customers.Infrastructure.IntegrationEvents.FFYCustomerRegisteredBusinessEvent}" />
public class Dynamics365FinanceCustomerRegisteredHandler : Dynamics365FinanceCustomerInformationHandler<Dynamics365FinanceCustomerRegistered>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerRegisteredHandler"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    public Dynamics365FinanceCustomerRegisteredHandler(
        ICustomerAggregateQueryService customerService,
        IDateTimeService dateTimeService,
        IOptions<OrganizationSettings> settings,
        ILogger<Dynamics365FinanceCustomerRegisteredHandler> logger)
        : base(
            customerService,
            dateTimeService, /*externalReferenceMapperService,*/
            settings,
            logger)
    {
    }
}