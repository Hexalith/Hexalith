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
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Application.Parties.Commands;
using Hexalith.Application.Parties.Services;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Configuration;
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
    private readonly ICustomerProjectionService _customerService;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<Dynamics365FinanceCustomerChangedHandler> _logger;
    private readonly IOptions<OrganizationSettings> _settings;
    private string _originId;
    private string _partitionId;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerChangedHandler"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    public Dynamics365FinanceCustomerChangedHandler(
        ICustomerProjectionService customerService,
        IDateTimeService dateTimeService,
        IOptions<OrganizationSettings> settings,
        ILogger<Dynamics365FinanceCustomerChangedHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultPartitionId);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultOriginId);
        _partitionId = settings.Value.DefaultPartitionId;
        _originId = settings.Value.DefaultOriginId;

        _customerService = customerService;
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
        string originId;
        string customerId;
        string companyId;
        Customer? customer = null;
        if (string.IsNullOrWhiteSpace(aggregateId))
        {
            // Ignore event without Hexalith external code if not a registration event
            LogMessageWithoutHexalithExternalCode(@event.TypeName, @event.BusinessEventLegalEntity, @event.Account);
            return [];
        }
        else
        {
            customer = await _customerService
                .GetCustomerAsync(aggregateId, cancellationToken)
                .ConfigureAwait(false);
            if (customer == null)
            {
                string[] parts = aggregateId.Split(Aggregate.Separator, 5);
                companyId = parts[2];
                originId = parts[3];
                customerId = parts[4];

                // TODO : Do a call to the customer aggregate service to get the customer
                // throw new InvalidOperationException($"Customer '{aggregateId}' not found while handling '{@event.BusinessEventLegalEntity}/{@event.Account}' event.");
            }
            else
            {
                originId = customer.OriginId;
                customerId = customer.Id;
                companyId = customer.CompanyId;
            }
        }

        string customerAggregateName = Customer.GetAggregateName();
        List<BaseCommand> commands = [];
        if (@event.ExternalReferences != null)
        {
            foreach (ExternalReference reference in @event.ExternalReferences)
            {
                AddExternalSystemReference mapExternalSystemReference = new(
                        _partitionId,
                        @event.BusinessEventLegalEntity,
                        reference.SystemId,
                        customerAggregateName,
                        reference.ExternalId,
                        aggregateId);
                commands.Add(mapExternalSystemReference);
            }
        }

        commands.Add(new AddExternalSystemReference(
                _partitionId,
                @event.BusinessEventLegalEntity.ToUpperInvariant(),
                _originId,
                customerAggregateName,
                @event.Account,
                aggregateId));

        ChangeCustomerInformation changeCustomer = new(
                   _partitionId,
                   companyId,
                   originId,
                   customerId,
                   @event.Name,
                   CustomerConverter.ToPartyType(@event.PartyType ?? nameof(PartyType.Person)),
                   @event.Contact,
                   @event.WarehouseId,
                   @event.CommissionSalesGroupId,
                   @event.GroupId,
                   @event.SalesCurrencyId,
                   _dateTimeService.UtcNow);

        commands.Add(changeCustomer);

        bool directDelivery = @event.InterCompanyDirectDelivery == "Yes";
        if (directDelivery)
        {
            commands.Add(new SelectIntercompanyDropshipDeliveryForCustomer(_partitionId, companyId, _originId, customerId));
        }
        else
        {
            commands.Add(new DeselectIntercompanyDropshipDeliveryForCustomer(_partitionId, companyId, _originId, customerId));
        }

        return commands;
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Event {EventType} for customer '{CompanyId}/{CustomerAccount}' without 'Hexalith' external code, is ignored.")]
    private partial void LogMessageWithoutHexalithExternalCode(string eventType, string companyId, string customerAccount);
}