// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 10-24-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="Dynamics365FinancePartnerInventoryItemBindingController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.Controllers;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;

using FluentValidation;

using Hexalith.Application.Organizations.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Controllers;
using Hexalith.Infrastructure.Dynamics365Finance.Dispatchers;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.BusinessEvents;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class SalesInvoicePostedBindingController.
/// Implements the <see cref="Dynamics365FinanceBindingController" />.
/// </summary>
/// <seealso cref="Dynamics365FinanceBindingController" />
public abstract class Dynamics365FinancePartnerInventoryItemBindingController : Dynamics365FinanceBindingController
{
    /// <summary>
    /// The add validator.
    /// </summary>
    private readonly IValidator<Dynamics365FinancePartnerInventoryItemAdded> _addedValidator;

    private readonly IOptions<Dynamics365FinanceInventoriesSettings> _inventoriesSettings;

    private readonly IValidator<Dynamics365FinancePartnerInventoryItemNameChanged> _nameChangedValidator;

    private readonly IValidator<Dynamics365FinancePartnerInventoryItemPriceChanged> _priceChangedValidator;

    /// <summary>
    /// The registered validator.
    /// </summary>
    private readonly IValidator<Dynamics365FinancePartnerInventoryItemRemoved> _removedValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinancePartnerInventoryItemBindingController"/> class.
    /// </summary>
    /// <param name="metadataValidator">The metadata validator.</param>
    /// <param name="registeredValidator">The registered validator.</param>
    /// <param name="changedValidator">The changed validator.</param>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="organizationSettings">The organization settings.</param>
    /// <param name="inventoriesSettings">The parties settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected Dynamics365FinancePartnerInventoryItemBindingController(
        IValidator<Dynamics365BusinessEventBase> metadataValidator,
        IValidator<Dynamics365FinancePartnerInventoryItemAdded> addedValidator,
        IValidator<Dynamics365FinancePartnerInventoryItemRemoved> removedValidator,
        IValidator<Dynamics365FinancePartnerInventoryItemNameChanged> nameChangedValidator,
        IValidator<Dynamics365FinancePartnerInventoryItemPriceChanged> priceChangedValidator,
        IDynamics365FinanceIntegrationEventProcessor eventProcessor,
        IHostEnvironment hostEnvironment,
        IOptions<OrganizationSettings> organizationSettings,
        IOptions<Dynamics365FinanceInventoriesSettings> inventoriesSettings,
        ILogger<Dynamics365FinancePartnerInventoryItemBindingController> logger)
        : base(metadataValidator, eventProcessor, hostEnvironment, organizationSettings, logger)
    {
        ArgumentNullException.ThrowIfNull(inventoriesSettings);
        ArgumentNullException.ThrowIfNull(addedValidator);
        ArgumentNullException.ThrowIfNull(removedValidator);
        ArgumentNullException.ThrowIfNull(nameChangedValidator);
        ArgumentNullException.ThrowIfNull(priceChangedValidator);
        _addedValidator = addedValidator;
        _inventoriesSettings = inventoriesSettings;
        _removedValidator = removedValidator;
        _nameChangedValidator = nameChangedValidator;
        _priceChangedValidator = priceChangedValidator;
    }

    /// <summary>
    /// Receive catalog item event.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    [Produces("application/json")]
    [HttpPost("d365fnopartnerinventoryitemsbinding")]
    public async Task<ActionResult> ReceiveCustomerEventAsync(
       [Swashbuckle.AspNetCore.Annotations.SwaggerRequestBody(Description ="Dynamics 365 finance partner inventory item business event", Required = true)]
       [FromBody] JsonElement message)
    {
        return await HandleEventAsync(message, CancellationToken.None)
                .ConfigureAwait(false);
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
        if (message is Dynamics365FinancePartnerInventoryItemAdded added)
        {
            _addedValidator.ValidateAndThrow(added);
            return;
        }

        if (message is Dynamics365FinancePartnerInventoryItemRemoved removed)
        {
            _removedValidator.ValidateAndThrow(removed);
            return;
        }

        if (message is Dynamics365FinancePartnerInventoryItemPriceChanged price)
        {
            _priceChangedValidator.ValidateAndThrow(price);
            return;
        }

        if (message is Dynamics365FinancePartnerInventoryItemNameChanged name)
        {
            _nameChangedValidator.ValidateAndThrow(name);
            return;
        }

        throw new ValidationException(
            $"Unsupported message type '{message.EventId}'. Expected:\n{new Dynamics365FinancePartnerInventoryItemAdded().TypeName} ({new Dynamics365FinancePartnerInventoryItemRemoved().TypeName}) or {new Dynamics365FinancePartnerInventoryItemNameChanged().TypeName} ({new Dynamics365FinancePartnerInventoryItemPriceChanged().TypeName})");
    }
}