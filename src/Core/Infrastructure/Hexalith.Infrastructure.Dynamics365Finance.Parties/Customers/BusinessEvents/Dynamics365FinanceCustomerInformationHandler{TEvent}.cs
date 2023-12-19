// ***********************************************************************
// Assembly         : Bspk.Customers
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerInformationHandler{TEvent}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
/// Class FFYCustomerInformationBusinessEventHandler.
/// Implements the <see cref="IntegrationEventHandler`1" />.
/// </summary>
/// <typeparam name="TEvent">The type of the t event.</typeparam>
/// <seealso cref="IntegrationEventHandler`1" />
public abstract partial class Dynamics365FinanceCustomerInformationHandler<TEvent> : IntegrationEventHandler<TEvent>
    where TEvent : Dynamics365FinanceCustomerInformationBusinessEvent
{
    private readonly ICustomerProjectionService _customerService;

    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    private readonly ILogger _logger;

    /// <summary>
    /// The origin identifier.
    /// </summary>
    private readonly string _originId;

    /// <summary>
    /// The external reference service.
    /// </summary>
    private readonly string _partitionId;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerInformationHandler{TEvent}"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected Dynamics365FinanceCustomerInformationHandler(
        ICustomerProjectionService customerService,
        IDateTimeService dateTimeService,
        IOptions<OrganizationSettings> settings,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);

        // ArgumentNullException.ThrowIfNull(externalReferenceMapperService);
        ArgumentNullException.ThrowIfNull(settings);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultPartitionId);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultOriginId);
        _partitionId = settings.Value.DefaultPartitionId;
        _originId = settings.Value.DefaultOriginId;
        _customerService = customerService;
        _dateTimeService = dateTimeService;
        _logger = logger;

        // _externalReferenceMapperService = externalReferenceMapperService;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(TEvent @event, CancellationToken cancellationToken)
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
        string partitionId;
        string companyId;
        if (string.IsNullOrWhiteSpace(aggregateId))
        {
            if (@event is not Dynamics365FinanceCustomerRegistered)
            {
                // Ignore event without Hexalith external code if not a registration event
                LogMessageWithoutHexalithExternalCode(@event.TypeName, @event.BusinessEventLegalEntity, @event.Account);
                return [];
            }

            originId = _originId;
            customerId = @event.Account;
            partitionId = _partitionId;
            companyId = @event.BusinessEventLegalEntity.ToUpperInvariant();
            aggregateId = Customer.GetAggregateId(_partitionId, companyId, _originId, customerId);
        }
        else
        {
            Customer? customer = await _customerService
                .GetCustomerAsync(aggregateId, cancellationToken)
                .ConfigureAwait(false);
            if (customer == null)
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
                originId = customer.OriginId;
                customerId = customer.Id;
                partitionId = customer.PartitionId;
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

        commands.Add(new RegisterOrChangeCustomer(
                   partitionId,
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
                   _dateTimeService.UtcNow));

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