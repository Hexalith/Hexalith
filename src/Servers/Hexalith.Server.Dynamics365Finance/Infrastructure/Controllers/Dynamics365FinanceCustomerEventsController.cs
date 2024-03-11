// <copyright file="Dynamics365FinanceCustomerEventsController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Server.Dynamics365Finance.Infrastructure.Controllers;

using FluentValidation;

using Hexalith.Application.Organizations.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Controllers;
using Hexalith.Infrastructure.Dynamics365Finance.Dispatchers;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Controller;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

/// <summary>
/// Class Dynamics365FinanceCustomerEventsController.
/// Implements the <see cref="Dynamics365FinanceBindingController" />.
/// </summary>
/// <seealso cref="Dynamics365FinanceBindingController" />
/// <remarks>
/// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerEventsController"/> class.
/// </remarks>
/// <param name="metadataValidator">The metadata validator.</param>
/// <param name="registeredValidator">The registered validator.</param>
/// <param name="changedValidator">The changed validator.</param>
/// <param name="eventProcessor">The event processor.</param>
/// <param name="hostEnvironment">The host environment.</param>
/// <param name="organizationSettings">The organization settings.</param>
/// <param name="partiesSettings">The parties settings.</param>
/// <param name="logger">The logger.</param>
[ApiController]
public class Dynamics365FinanceCustomerEventsController(
    IValidator<Dynamics365BusinessEventBase> metadataValidator,
    IValidator<Dynamics365FinanceCustomerRegistered> registeredValidator,
    IValidator<Dynamics365FinanceCustomerChanged> changedValidator,
    IDynamics365FinanceIntegrationEventProcessor eventProcessor,
    IHostEnvironment hostEnvironment,
    IOptions<OrganizationSettings> organizationSettings,
    IOptions<Dynamics365FinancePartiesSettings> partiesSettings,
    ILogger<Dynamics365FinanceCustomerBindingController> logger)
    : Dynamics365FinanceCustomerBindingController(
        metadataValidator,
        registeredValidator,
        changedValidator,
        eventProcessor,
        hostEnvironment,
        organizationSettings,
        partiesSettings,
        logger)
{
}