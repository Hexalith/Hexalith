// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerChangedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Application.Parties.Services;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerEventHandler.
/// Implements the <see cref="Application.Events.IntegrationEventHandler{Customers.Infrastructure.IntegrationEvents.FFYCustomerChangedBusinessEvent}" />.
/// </summary>
/// <seealso cref="Application.Events.IntegrationEventHandler{Customers.Infrastructure.IntegrationEvents.FFYCustomerChangedBusinessEvent}" />
public class Dynamics365FinanceCustomerChangedHandler : Dynamics365FinanceCustomerInformationHandler<Dynamics365FinanceCustomerChanged>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerChangedHandler"/> class.
    /// </summary>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="customerService">The customer service.</param>
    /// <param name="externalReferenceMapperService">The external reference service.</param>
    /// <param name="settings">The settings.</param>
    public Dynamics365FinanceCustomerChangedHandler(
        IDateTimeService dateTimeService,
        ICustomerQueryService customerService,
        IExternalReferenceMapperService externalReferenceMapperService,
        IOptions<OrganizationSettings> settings)
        : base(
            dateTimeService,
            customerService,
            externalReferenceMapperService,
            settings)
    {
    }
}