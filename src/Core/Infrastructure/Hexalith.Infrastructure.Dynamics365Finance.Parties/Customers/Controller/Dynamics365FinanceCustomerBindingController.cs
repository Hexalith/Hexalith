// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 10-24-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerBindingController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Controller;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;

using FluentValidation;

using Hexalith.Application.Organizations.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Controllers;
using Hexalith.Infrastructure.Dynamics365Finance.Dispatchers;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Class SalesInvoicePostedBindingController.
/// Implements the <see cref="Dynamics365FinanceBindingController" />.
/// </summary>
/// <seealso cref="Dynamics365FinanceBindingController" />
public abstract partial class Dynamics365FinanceCustomerBindingController : Dynamics365FinanceBindingController
{
    /// <summary>
    /// The add validator.
    /// </summary>
    private readonly IValidator<Dynamics365FinanceCustomerChanged> _changedValidator;

    private readonly IOptions<Dynamics365FinancePartiesSettings> _partiesSettings;

    /// <summary>
    /// The registered validator.
    /// </summary>
    private readonly IValidator<Dynamics365FinanceCustomerRegistered> _registeredValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerBindingController"/> class.
    /// </summary>
    /// <param name="metadataValidator">The metadata validator.</param>
    /// <param name="registeredValidator">The registered validator.</param>
    /// <param name="changedValidator">The changed validator.</param>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="organizationSettings">The organization settings.</param>
    /// <param name="partiesSettings">The parties settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected Dynamics365FinanceCustomerBindingController(
        IValidator<Dynamics365BusinessEventBase> metadataValidator,
        IValidator<Dynamics365FinanceCustomerRegistered> registeredValidator,
        IValidator<Dynamics365FinanceCustomerChanged> changedValidator,
        IDynamics365FinanceIntegrationEventProcessor eventProcessor,
        IHostEnvironment hostEnvironment,
        IOptions<OrganizationSettings> organizationSettings,
        IOptions<Dynamics365FinancePartiesSettings> partiesSettings,
        ILogger<Dynamics365FinanceCustomerBindingController> logger)
        : base(metadataValidator, eventProcessor, hostEnvironment, organizationSettings, logger)
    {
        ArgumentNullException.ThrowIfNull(registeredValidator);
        ArgumentNullException.ThrowIfNull(changedValidator);
        ArgumentNullException.ThrowIfNull(partiesSettings);

        _changedValidator = changedValidator;
        _partiesSettings = partiesSettings;
        _registeredValidator = registeredValidator;
    }

    /// <summary>
    /// Receive catalog item event.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    [Produces("application/json")]
    [HttpPost("d365fnocustomersbinding")]
    public async Task<ActionResult> ReceiveCustomerEventAsync(
       [SwaggerRequestBody(Description ="Dynamics 365 finance customer business event", Required = true)]
       [FromBody] JsonElement message)
    {
        if (_partiesSettings.Value.Customers?.ReceiveCustomersFromErpEnabled == true)
        {
            return await HandleEventAsync(message, CancellationToken.None)
                .ConfigureAwait(false);
        }

        LogMessageIsIgnoredInformation(Logger);
        return Ok();
    }

    /// <summary>
    /// Validates the message and if not successful throws a validation exception <see cref="ValidationException" /> .
    /// </summary>
    /// <param name="message">The message.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    /// <exception cref="FluentValidation.ValidationException">Unsupported message type '{message.EventId}'. Expected:\n{nameof(Dynamics365FinanceCustomerChanged)} ({new Dynamics365FinanceCustomerChanged().TypeName}) or {nameof(Dynamics365FinanceCustomerRegistered)} ({new Dynamics365FinanceCustomerRegistered().TypeName}).</exception>
    /// <exception cref="Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents.Dynamics365FinanceCustomerChanged">error.</exception>
    /// <exception cref="Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents.Dynamics365FinanceCustomerRegistered">error 2.</exception>
    protected override void ValidateAndThrow([NotNull] Dynamics365BusinessEventBase message)
    {
        ArgumentNullException.ThrowIfNull(message);
        if (message is Dynamics365FinanceCustomerChanged changed)
        {
            _changedValidator.ValidateAndThrow(changed);
            return;
        }

        if (message is Dynamics365FinanceCustomerRegistered registered)
        {
            _registeredValidator.ValidateAndThrow(registered);
            return;
        }

        throw new ValidationException(
            $"Unsupported message type '{message.EventId}'. Expected:\n{nameof(Dynamics365FinanceCustomerChanged)} ({new Dynamics365FinanceCustomerChanged().TypeName}) or {nameof(Dynamics365FinanceCustomerRegistered)} ({new Dynamics365FinanceCustomerRegistered().TypeName})");
    }

    [LoggerMessage(
            EventId = 1,
        Level = LogLevel.Information,
        Message = "Receive customer event from Dynamics 365 Finance and Operations is disabled. Business event is ignored.")]
    private static partial void LogMessageIsIgnoredInformation(ILogger logger);
}