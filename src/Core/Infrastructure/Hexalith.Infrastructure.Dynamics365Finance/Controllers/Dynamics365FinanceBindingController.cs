// <copyright file="Dynamics365FinanceBindingController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Controllers;

using System.Runtime.Serialization;
using System.Text.Json;

using FluentValidation;

using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Configuration;
using Hexalith.Extensions.Errors;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Dispatchers;
using Hexalith.Infrastructure.WebApis.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class Dynamics365FinanceEventBindingController.
/// Implements the <see cref="ControllerBase" />.
/// </summary>
/// <seealso cref="ControllerBase" />
public abstract class Dynamics365FinanceBindingController : BindingController
{
    private readonly string _defaultCompanyId;
    private readonly string _defaultOriginId;
    private readonly string _defaultPartitionId;

    /// <summary>
    /// The event validator.
    /// </summary>
    private readonly IValidator<Dynamics365BusinessEventBase> _eventValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceBindingController"/> class.
    /// </summary>
    /// <param name="metadataValidator">The metadata validator.</param>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="organizationSettings">The organization settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected Dynamics365FinanceBindingController(
        IValidator<Dynamics365BusinessEventBase> metadataValidator,
        IDynamics365FinanceIntegrationEventProcessor eventProcessor,
        IHostEnvironment hostEnvironment,
        IOptions<OrganizationSettings> organizationSettings,
        ILogger logger)
        : base(eventProcessor, hostEnvironment, logger)
    {
        ArgumentNullException.ThrowIfNull(metadataValidator);
        ArgumentNullException.ThrowIfNull(organizationSettings);
        OrganizationSettings settings = organizationSettings.Value;
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.DefaultPartitionId);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.DefaultCompanyId);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.DefaultOriginId);
        _defaultPartitionId = settings.DefaultPartitionId;
        _defaultOriginId = settings.DefaultOriginId;
        _defaultCompanyId = settings.DefaultCompanyId;
        _eventValidator = metadataValidator;
    }

    /// <inheritdoc/>
    protected override (IEvent Event, IMetadata Metadata) DeserializeAndValidate(JsonElement message)
    {
        try
        {
            Dynamics365BusinessEventBase businessEvent = message.Deserialize<Dynamics365BusinessEventBase>() ?? throw new InvalidOperationException("Deserialized business event is null.");
            businessEvent.PartitionId = string.IsNullOrWhiteSpace(businessEvent.PartitionId) ? _defaultPartitionId : businessEvent.PartitionId;
            businessEvent.BusinessEventLegalEntity = string.IsNullOrWhiteSpace(businessEvent.BusinessEventLegalEntity) ? _defaultCompanyId : businessEvent.BusinessEventLegalEntity;
            businessEvent.OriginId = string.IsNullOrWhiteSpace(businessEvent.OriginId) ? _defaultOriginId : businessEvent.OriginId;

            _eventValidator.ValidateAndThrow(businessEvent);
            ValidateAndThrow(businessEvent);
            return (businessEvent, businessEvent);
        }
        catch (ValidationException ex)
        {
            throw new ApplicationErrorException(
                new EventValidationFailed(message.GetRawText(), ex),
                ex);
        }
        catch (SerializationException ex)
        {
            throw new ApplicationErrorException(
                new EventDeserializationFailed(message.GetRawText(), ex),
                ex);
        }
        catch (JsonException ex)
        {
            throw new ApplicationErrorException(
                new EventDeserializationFailed(message.GetRawText(), ex),
                ex);
        }
    }

    /// <summary>
    /// Validates the message and if not successful throws a validation exception <see cref="ValidationException" /> .
    /// </summary>
    /// <param name="message">The message.</param>
    protected abstract void ValidateAndThrow(Dynamics365BusinessEventBase message);
}