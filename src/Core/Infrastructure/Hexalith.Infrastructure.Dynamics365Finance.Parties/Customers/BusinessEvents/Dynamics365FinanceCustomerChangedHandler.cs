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

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.ExternalSystems.Commands;
using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Application.Parties.Commands;
using Hexalith.Application.Parties.Helpers;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Projections;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerEventHandler.
/// Implements the <see cref="Application.Events.IntegrationEventHandler{Customers.Infrastructure.IntegrationEvents.FFYCustomerChangedBusinessEvent}" />.
/// </summary>
/// <seealso cref="Application.Events.IntegrationEventHandler{Customers.Infrastructure.IntegrationEvents.FFYCustomerChangedBusinessEvent}" />
public partial class Dynamics365FinanceCustomerChangedHandler : IntegrationEventHandler<Dynamics365FinanceCustomerChanged>
{
    private readonly IActorProjectionFactory<CustomerRegistered> _customerService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IDynamics365FinanceClient<CustomerBase> _erpCustomerBaseService;
    private readonly IDynamics365FinanceClient<CustomerV3> _erpCustomerV3Service;
    private readonly IExternalReferenceMapperService _externalReferenceMapperService;
    private readonly ILogger<Dynamics365FinanceCustomerChangedHandler> _logger;
    private readonly IOptions<OrganizationSettings> _settings;
    private string _originId;
    private string _partitionId;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerChangedHandler"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="erpCustomerV3Service">The erp customer v3 service.</param>
    /// <param name="erpCustomerBaseService">The erp customer base service.</param>
    /// <param name="externalReferenceMapperService">The external reference mapper service.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public Dynamics365FinanceCustomerChangedHandler(
        IActorProjectionFactory<CustomerRegistered> customerService,
        IDynamics365FinanceClient<CustomerV3> erpCustomerV3Service,
        IDynamics365FinanceClient<CustomerBase> erpCustomerBaseService,
        IExternalReferenceMapperService externalReferenceMapperService,
        IDateTimeService dateTimeService,
        IOptions<OrganizationSettings> settings,
        ILogger<Dynamics365FinanceCustomerChangedHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(customerService);
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
        _externalReferenceMapperService = externalReferenceMapperService;
        _dateTimeService = dateTimeService;
        _settings = settings;
        _logger = logger;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(Dynamics365FinanceCustomerChanged @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(@event.Contact);
        ArgumentNullException.ThrowIfNull(@event.Contact.PostalAddress);
        ArgumentException.ThrowIfNullOrEmpty(@event.Account);
        ArgumentException.ThrowIfNullOrEmpty(@event.Name);
        ArgumentException.ThrowIfNullOrEmpty(@event.BusinessEventLegalEntity);

        string? aggregateId = @event
            .ExternalReferences?
            .Where(p => p.SystemId == nameof(Hexalith))
            .FirstOrDefault()?
            .ExternalId;
        string partitionId;
        string originId;
        string customerId;
        string companyId;
        if (string.IsNullOrWhiteSpace(aggregateId))
        {
            // Ignore event without Hexalith external code if not a registration event
            LogMessageWithoutHexalithExternalCode(@event.TypeName, @event.BusinessEventLegalEntity, @event.Account);
            return [];
        }

        CustomerRegistered? lastState = await _customerService
            .GetStateAsync(@event.AggregateId, cancellationToken)
            .ConfigureAwait(false);

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

        ChangeCustomerInformation changeCustomer = await GetCustomerChangedAsync(
                partitionId,
                companyId,
                originId,
                customerId,
                lastState,
                @event,
                cancellationToken)
            .ConfigureAwait(false);

        string? hasChanges = (lastState == null) ? $"No current state found for '{aggregateId}', applying changes." : changeCustomer.HasChanges(lastState);
        if (hasChanges == null)
        {
            return [];
        }

        LogEventHasChangesInformation(@event.BusinessEventLegalEntity, @event.Account, hasChanges);

        List<BaseCommand> commands = [];
        if (@event.ExternalReferences != null)
        {
            foreach (ExternalReference reference in @event.ExternalReferences)
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
                @event.Account,
                changeCustomer.AggregateId));

        commands.Add(changeCustomer);

        bool directDelivery = @event.InterCompanyDirectDelivery == "Yes";
        if (directDelivery)
        {
            commands.Add(new SelectIntercompanyDropshipDeliveryForCustomer(partitionId, companyId, originId, customerId));
        }
        else
        {
            commands.Add(new DeselectIntercompanyDropshipDeliveryForCustomer(partitionId, companyId, originId, customerId));
        }

        return commands;
    }

    private async Task<ChangeCustomerInformation> GetCustomerChangedAsync(
        string partitionId,
        string companyId,
        string originId,
        string customerId,
        CustomerRegistered? lastState,
        Dynamics365FinanceCustomerChanged @event,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(@event.BusinessEventLegalEntity);
        ArgumentException.ThrowIfNullOrEmpty(@event.Account);
        CustomerBase customerBase = await _erpCustomerBaseService
            .GetSingleAsync(
                new CustomerAccountKey(@event.BusinessEventLegalEntity, @event.Account),
                cancellationToken).ConfigureAwait(false);
        CustomerV3 customerV3 = await _erpCustomerV3Service
            .GetSingleAsync(
                new CustomerAccountKey(@event.BusinessEventLegalEntity, @event.Account),
                cancellationToken).ConfigureAwait(false);
        int month = (customerBase.PersonBirthMonth == null) ? 1 : (int)customerBase.PersonBirthMonth;
        DateTimeOffset birthDate = new(
            customerBase.PersonBirthYear ?? 1,
            month,
            customerBase.PersonBirthDay ?? 1,
            0,
            0,
            0,
            TimeSpan.Zero);

        return customerV3.ToChangeCustomerCommand(
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

    private PostalAddress? GetPostalAddress(PostalAddress? previousAddress, PostalAddress? newAddress)
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

    [LoggerMessage(
               EventId = 0,
               Level = LogLevel.Information,
               Message = "Event {BusinessEventLegalEntity}/{Account} has changes :\n{HasChanges}.")]
    private partial void LogEventHasChangesInformation(string businessEventLegalEntity, string account, string hasChanges);

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Event {EventType} for customer '{CompanyId}/{CustomerAccount}' without 'Hexalith' external code, is ignored.")]
    private partial void LogMessageWithoutHexalithExternalCode(string eventType, string companyId, string customerAccount);
}