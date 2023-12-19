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

using Hexalith.Application.Metadatas;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Projections;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Controllers;
using Hexalith.Infrastructure.Dynamics365Finance.Dispatchers;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Projections;

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
public class Dynamics365FinanceCustomerBindingController : Dynamics365FinanceBindingController
{
    /// <summary>
    /// The add validator.
    /// </summary>
    private readonly IValidator<Dynamics365FinanceCustomerChanged> _changedValidator;

    private readonly IActorProjectionFactory<Dynamics365FinanceCustomerState> _erpState;

    /// <summary>
    /// The registered validator.
    /// </summary>
    private readonly IValidator<Dynamics365FinanceCustomerRegistered> _registeredValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerBindingController"/> class.
    /// </summary>
    /// <param name="erpState">State of the erp.</param>
    /// <param name="metadataValidator">The metadata validator.</param>
    /// <param name="registeredValidator">The registered validator.</param>
    /// <param name="changedValidator">The changed validator.</param>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="organizationSettings">The organization settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public Dynamics365FinanceCustomerBindingController(
        IActorProjectionFactory<Dynamics365FinanceCustomerState> erpState,
        IValidator<Dynamics365BusinessEventBase> metadataValidator,
        IValidator<Dynamics365FinanceCustomerRegistered> registeredValidator,
        IValidator<Dynamics365FinanceCustomerChanged> changedValidator,
        IDynamics365FinanceIntegrationEventProcessor eventProcessor,
        IHostEnvironment hostEnvironment,
        IOptions<OrganizationSettings> organizationSettings,
        ILogger<Dynamics365FinanceCustomerBindingController> logger)
        : base(metadataValidator, eventProcessor, hostEnvironment, organizationSettings, logger)
    {
        ArgumentNullException.ThrowIfNull(erpState);
        ArgumentNullException.ThrowIfNull(registeredValidator);
        ArgumentNullException.ThrowIfNull(changedValidator);
        _changedValidator = changedValidator;
        _erpState = erpState;
        _registeredValidator = registeredValidator;
    }

    /// <summary>
    /// Receive catalog item event.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    [Produces("application/json")]
    [HttpPost("d365fno-customers-binding")]
    public async Task<ActionResult> ReceiveCustomerEventAsync(
       [SwaggerRequestBody(Description ="Dynamics 365 finance customer business event", Required = true)]
       [FromBody] JsonElement message) => await HandleEventAsync(message, CancellationToken.None).ConfigureAwait(false);

    /// <inheritdoc/>
    protected override async Task PostHandleEventProcessAsync(IEvent @event, IMetadata metadata, CancellationToken cancellationToken)
    {
        if (@event is Dynamics365FinanceCustomerInformationBusinessEvent businessEvent)
        {
            await _erpState.SetStateAsync(
                    @event.AggregateId,
                    Dynamics365FinanceCustomerState.Create(businessEvent),
                    cancellationToken)
                .ConfigureAwait(false);
        }
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
}