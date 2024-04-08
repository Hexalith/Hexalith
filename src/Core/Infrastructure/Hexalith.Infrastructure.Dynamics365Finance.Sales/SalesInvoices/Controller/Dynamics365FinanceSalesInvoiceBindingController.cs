// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Sales
// Author           : Jérôme Piquot
// Created          : 10-24-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceSalesInvoiceBindingController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Controller;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;

using FluentValidation;

using Hexalith.Application.Organizations.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Controllers;
using Hexalith.Infrastructure.Dynamics365Finance.Dispatchers;
using Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.IntegrationEvents;

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
public abstract partial class Dynamics365FinanceSalesInvoiceBindingController : Dynamics365FinanceBindingController
{
    private readonly IOptions<Dynamics365FinanceSalesSettings> _partiesSettings;

    /// <summary>
    /// The registered validator.
    /// </summary>
    private readonly IValidator<SalesInvoicePostedBusinessEvent> _registeredValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceSalesInvoiceBindingController"/> class.
    /// </summary>
    /// <param name="metadataValidator">The metadata validator.</param>
    /// <param name="registeredValidator">The registered validator.</param>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="organizationSettings">The organization settings.</param>
    /// <param name="partiesSettings">The parties settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected Dynamics365FinanceSalesInvoiceBindingController(
        IValidator<Dynamics365BusinessEventBase> metadataValidator,
        IValidator<SalesInvoicePostedBusinessEvent> registeredValidator,
        IDynamics365FinanceIntegrationEventProcessor eventProcessor,
        IHostEnvironment hostEnvironment,
        IOptions<OrganizationSettings> organizationSettings,
        IOptions<Dynamics365FinanceSalesSettings> partiesSettings,
        ILogger<Dynamics365FinanceSalesInvoiceBindingController> logger)
        : base(metadataValidator, eventProcessor, hostEnvironment, organizationSettings, logger)
    {
        ArgumentNullException.ThrowIfNull(registeredValidator);
        ArgumentNullException.ThrowIfNull(partiesSettings);

        _partiesSettings = partiesSettings;
        _registeredValidator = registeredValidator;
    }

    /// <summary>
    /// Receive catalog item event.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    [Produces("application/json")]
    [HttpPost("d365fnosalesinvoicesbinding")]
    public async Task<ActionResult> ReceiveSalesInvoiceEventAsync(
       [SwaggerRequestBody(Description ="Dynamics 365 finance customer business event", Required = true)]
       [FromBody] JsonElement message)
    {
        if (_partiesSettings.Value.Sales?.ReceiveSalesInvoicesFromErpEnabled == true)
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
    /// <exception cref="FluentValidation.ValidationException">Unsupported message type '{message.EventId}'. Expected:\n{nameof(Dynamics365FinanceSalesInvoiceChanged)} ({new Dynamics365FinanceSalesInvoiceChanged().TypeName}) or {nameof(Dynamics365FinanceSalesInvoiceRegistered)} ({new Dynamics365FinanceSalesInvoiceRegistered().TypeName}).</exception>
    /// <exception cref="Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.BusinessEvents.Dynamics365FinanceSalesInvoiceChanged">error.</exception>
    /// <exception cref="Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.BusinessEvents.Dynamics365FinanceSalesInvoiceRegistered">error 2.</exception>
    protected override void ValidateAndThrow([NotNull] Dynamics365BusinessEventBase message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message is SalesInvoicePostedBusinessEvent registered)
        {
            _registeredValidator.ValidateAndThrow(registered);
            return;
        }

        throw new ValidationException(
            $"Unsupported message type '{message.EventId}'. Expected: {new SalesInvoicePostedBusinessEvent().TypeName})");
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Receive sales invoice event from Dynamics 365 Finance and Operations is disabled. Business event is ignored.")]
    private static partial void LogMessageIsIgnoredInformation(ILogger logger);
}