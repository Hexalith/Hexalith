// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-21-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerChangedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.ExternalSystems.Commands;
using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Application.Parties.Commands;
using Hexalith.Application.Parties.Helpers;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Projections;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Filters;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerEventHandler.
/// Implements the <see cref="Application.Events.IntegrationEventHandlerBase{Customers.Infrastructure.IntegrationEvents.FFYCustomerChangedBusinessEvent}" />.
/// </summary>
/// <seealso cref="Application.Events.IntegrationEventHandlerBase{Customers.Infrastructure.IntegrationEvents.FFYCustomerChangedBusinessEvent}" />
public partial class Dynamics365FinanceCustomerChangedHandler : IntegrationEventHandlerBase<Dynamics365FinanceCustomerChanged>
{
    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly IActorProjectionFactory<Customer> _customerService;

    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// The erp customer base service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerBase> _erpCustomerBaseService;

    /// <summary>
    /// The erp customer v3 service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerV3> _erpCustomerV3Service;

    /// <summary>
    /// The erp external code.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerExternalSystemCode> _erpExternalCodeService;

    /// <summary>
    /// The external reference mapper service.
    /// </summary>
    private readonly IExternalReferenceMapperService _externalReferenceMapperService;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<Dynamics365FinanceCustomerChangedHandler> _logger;

    /// <summary>
    /// The origin identifier.
    /// </summary>
    private readonly string _originId;

    /// <summary>
    /// The partition identifier.
    /// </summary>
    private readonly string _partitionId;

    /// <summary>
    /// The settings.
    /// </summary>
    private readonly IOptions<OrganizationSettings> _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerChangedHandler" /> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="erpCustomerV3Service">The erp customer v3 service.</param>
    /// <param name="erpCustomerBaseService">The erp customer base service.</param>
    /// <param name="erpExternalCodeService">The erp external code.</param>
    /// <param name="externalReferenceMapperService">The external reference mapper service.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public Dynamics365FinanceCustomerChangedHandler(
        IActorProjectionFactory<Customer> customerService,
        IDynamics365FinanceClient<CustomerV3> erpCustomerV3Service,
        IDynamics365FinanceClient<CustomerBase> erpCustomerBaseService,
        IDynamics365FinanceClient<CustomerExternalSystemCode> erpExternalCodeService,
        IExternalReferenceMapperService externalReferenceMapperService,
        IDateTimeService dateTimeService,
        IOptions<OrganizationSettings> settings,
        ILogger<Dynamics365FinanceCustomerChangedHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(erpCustomerV3Service);
        ArgumentNullException.ThrowIfNull(erpCustomerBaseService);
        ArgumentNullException.ThrowIfNull(erpExternalCodeService);
        ArgumentNullException.ThrowIfNull(externalReferenceMapperService);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);

        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultPartitionId);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultOriginId);
        _partitionId = settings.Value.DefaultPartitionId;
        _originId = settings.Value.DefaultOriginId;
        _customerService = customerService;
        _erpCustomerV3Service = erpCustomerV3Service;
        _erpCustomerBaseService = erpCustomerBaseService;
        _erpExternalCodeService = erpExternalCodeService;
        _externalReferenceMapperService = externalReferenceMapperService;
        _dateTimeService = dateTimeService;
        _settings = settings;
        _logger = logger;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(Dynamics365FinanceCustomerChanged baseEvent, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(baseEvent.Contact);
        ArgumentNullException.ThrowIfNull(baseEvent.Contact.PostalAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseEvent.Account);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseEvent.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseEvent.BusinessEventLegalEntity);

        string? aggregateId = baseEvent
            .ExternalReferences?
            .Where(p => p.SystemId == nameof(Hexalith))
            .FirstOrDefault()?
            .ExternalId;
        CustomerBase customerBase = await _erpCustomerBaseService
            .GetSingleAsync(
                new CustomerAccountKey(baseEvent.BusinessEventLegalEntity, baseEvent.Account),
                cancellationToken).ConfigureAwait(false);
        bool externalReferenceFound = true;
        if (string.IsNullOrWhiteSpace(aggregateId))
        {
            // The customer does not have a Hexalith identifier in the external codes.
            if (customerBase.NameAlias != null && customerBase
                .NameAlias
                .StartsWith(Dynamics365FinancePartiesConstants.CreatingCustomerStamp, StringComparison.OrdinalIgnoreCase))
            {
                // Ignore events coming from the Hexalith customer creation process.
                LogMessageFromHexalithCustomerCreation(baseEvent.TypeName, baseEvent.BusinessEventLegalEntity, baseEvent.Account, customerBase.NameAlias);
                return [];
            }

            IEnumerable<CustomerExternalSystemCode> ids = await _erpExternalCodeService
                .GetAsync(
                    new CustomerExternalCodeByAccountFilter(
                        baseEvent.BusinessEventLegalEntity,
                        nameof(Hexalith),
                        baseEvent.Account),
                    cancellationToken)
                .ConfigureAwait(false);

            aggregateId = ids.FirstOrDefault()?.ExternalCode;
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                externalReferenceFound = false;
                aggregateId = await _externalReferenceMapperService
                    .GetAggregateIdAsync(
                        Customer.GetAggregateName(),
                        _partitionId,
                        baseEvent.BusinessEventLegalEntity.ToUpperInvariant(),
                        _originId,
                        baseEvent.Account,
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                aggregateId = Customer.GetAggregateId(
                    _partitionId,
                    baseEvent.BusinessEventLegalEntity.ToUpperInvariant(),
                    _originId,
                    baseEvent.Account);
            }
        }

        CustomerV3 customerV3 = await _erpCustomerV3Service
            .GetSingleAsync(
                new CustomerAccountKey(baseEvent.BusinessEventLegalEntity, baseEvent.Account),
                cancellationToken).ConfigureAwait(false);

        Customer? lastState = await _customerService
            .GetStateAsync(baseEvent.AggregateId, cancellationToken)
            .ConfigureAwait(false);

        string partitionId;
        string originId;
        string customerId;
        string companyId;
        if (lastState == null)
        {
            string[] parts = aggregateId.Split(Aggregate.Separator, 5);
            partitionId = parts[1];
            companyId = parts[2];
            originId = parts[3];
            customerId = parts[4];

            // TODO : Do a call to the customer aggregate service to get the customer
            // throw new InvalidOperationException($"Customer '{aggregateId}' not found while handling '{@event.BusinessEventLegalEntity}/{@event.Account}' event.");
        }
        else
        {
            partitionId = lastState.PartitionId;
            originId = lastState.OriginId;
            customerId = lastState.Id;
            companyId = lastState.CompanyId;
        }

        if (!externalReferenceFound)
        {
            _ = await _erpExternalCodeService.PostAsync(
                    new CustomerExternalSystemCode(
                        null,
                        baseEvent.BusinessEventLegalEntity,
                        nameof(Hexalith),
                        baseEvent.Account,
                        aggregateId),
                    cancellationToken)
                .ConfigureAwait(false);
        }

        RegisterOrChangeCustomer changeCustomer = GetCustomerChanged(
                partitionId,
                companyId,
                originId,
                customerId,
                lastState,
                baseEvent,
                customerBase,
                customerV3);

        string? hasChanges = (lastState == null)
            ? $"No current state found for '{aggregateId}', applying changes."
            : changeCustomer.HasChanges(lastState.ToCustomerRegistered());

        List<BaseCommand> commands = [];

        if (hasChanges != null)
        {
            LogEventHasChangesInformation(baseEvent.BusinessEventLegalEntity, baseEvent.Account, hasChanges);
            commands.Add(changeCustomer);
        }

        bool directDelivery = baseEvent.InterCompanyDirectDelivery == "Yes";
        if (directDelivery != lastState?.IntercompanyDropship)
        {
            LogEventHasChangesInformation(baseEvent.BusinessEventLegalEntity, baseEvent.Account, $"DirectDelivery = {directDelivery}");
            if (directDelivery)
            {
                commands.Add(new SelectIntercompanyDropshipDeliveryForCustomer(partitionId, companyId, originId, customerId));
            }
            else
            {
                commands.Add(new DeselectIntercompanyDropshipDeliveryForCustomer(partitionId, companyId, originId, customerId));
            }
        }

        if (baseEvent.ExternalReferences != null)
        {
            foreach (ExternalReference reference in baseEvent.ExternalReferences)
            {
                AddExternalSystemReference mapExternalSystemReference = new(
                        partitionId,
                        companyId,
                        reference.SystemId,
                        changeCustomer.AggregateName,
                        reference.ExternalId,
                        changeCustomer.AggregateId);
                commands.Add(mapExternalSystemReference);
            }
        }

        commands.Add(new AddExternalSystemReference(
                partitionId,
                companyId,
                _originId,
                changeCustomer.AggregateName,
                baseEvent.Account,
                changeCustomer.AggregateId));

        return commands;
    }

    /// <summary>
    /// Gets the postal address.
    /// </summary>
    /// <param name="previousAddress">The previous address.</param>
    /// <param name="newAddress">The new address.</param>
    /// <returns>Hexalith.Domain.ValueObjets.PostalAddress?.</returns>
    private static PostalAddress? GetPostalAddress(PostalAddress? previousAddress, PostalAddress? newAddress)
    {
        if (previousAddress == null)
        {
            return (newAddress == null) ? null : new PostalAddress(newAddress);
        }

        if (newAddress == null)
        {
            return previousAddress;
        }

        PostalAddress address = new(newAddress)
        {
            CountyId = newAddress.CountyId ?? previousAddress.CountyId,
            StateId = newAddress.StateId ?? previousAddress.StateId,
            CountryId = newAddress.CountryId ?? previousAddress.CountryId,
            City = newAddress.City ?? previousAddress.City,
            Street = newAddress.Street ?? previousAddress.Street,
            ZipCode = newAddress.ZipCode ?? previousAddress.ZipCode,
            StreetNumber = newAddress.StreetNumber ?? previousAddress.StreetNumber,
            CountryName = newAddress.CountryName ?? previousAddress.CountryName,
            StateName = newAddress.StateName ?? previousAddress.StateName,
            Name = newAddress.Name ?? previousAddress.Name,
            CountryIso2 = newAddress.CountryIso2 ?? previousAddress.CountryIso2,
            Description = newAddress.Description ?? previousAddress.Description,
            PostBox = newAddress.PostBox ?? previousAddress.PostBox,
        };
        return address;
    }

    /// <summary>
    /// Gets the customer changed.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="lastState">The last state.</param>
    /// <param name="event">The event.</param>
    /// <param name="customerBase">The customer base.</param>
    /// <param name="customerV3">The customer v3.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Hexalith.Application.Parties.Commands.RegisterOrChangeCustomer.</returns>
    private RegisterOrChangeCustomer GetCustomerChanged(
        string partitionId,
        string companyId,
        string originId,
        string customerId,
        Customer? lastState,
        Dynamics365FinanceCustomerChanged @event,
        CustomerBase customerBase,
        CustomerV3 customerV3)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@event.BusinessEventLegalEntity);
        ArgumentException.ThrowIfNullOrWhiteSpace(@event.Account);
        int year = (customerBase.PersonBirthYear == null) ? 0 : customerBase.PersonBirthYear.Value;
        int month = (customerBase.PersonBirthMonth == null) ? 0 : (int)customerBase.PersonBirthMonth.Value;
        int day = (customerBase.PersonBirthDay == null) ? 0 : customerBase.PersonBirthDay.Value;
        DateTimeOffset birthDate = year == 0 || month == 0 || day == 0
            ? new(1900, 1, 1, 0, 0, 0, TimeSpan.Zero)
            : new DateTimeOffset(
            year,
            month,
            day,
            0,
            0,
            0,
            TimeSpan.Zero);
        return customerV3.ToRegisterOrChangeCustomerCommand(
                partitionId,
                companyId,
                originId,
                customerId,
                _dateTimeService.Now,
                lastState?.Contact?.PostalAddress?.PostBox,
                lastState?.Contact?.PostalAddress?.StateName,
                lastState?.Contact?.PostalAddress?.CountryName,
                lastState?.Contact?.Phone,
                lastState?.Contact?.Mobile,
                customerBase.PersonPersonalTitle,
                birthDate);
    }

    /// <summary>
    /// Logs the event has changes information.
    /// </summary>
    /// <param name="businessEventLegalEntity">The business event legal entity.</param>
    /// <param name="account">The account.</param>
    /// <param name="hasChanges">The has changes.</param>
    [LoggerMessage(
               EventId = 0,
               Level = LogLevel.Information,
               Message = "Event {BusinessEventLegalEntity}/{Account} has changes :\n{HasChanges}.")]
    private partial void LogEventHasChangesInformation(string businessEventLegalEntity, string account, string hasChanges);

    /// <summary>
    /// Logs the message from hexalith customer creation.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="customerAccount">The customer account.</param>
    /// <param name="creationStamp">The creation stamp.</param>
    [LoggerMessage(
        EventId = 1,
    Level = LogLevel.Information,
        Message = "Event '{EventType}' for customer '{CompanyId}/{CustomerAccount}' is ignored as it is currently being processed for creation by Hexalith. Creation stamp is : '{CreationStamp}'")]
    private partial void LogMessageFromHexalithCustomerCreation(string eventType, string companyId, string customerAccount, string creationStamp);
}