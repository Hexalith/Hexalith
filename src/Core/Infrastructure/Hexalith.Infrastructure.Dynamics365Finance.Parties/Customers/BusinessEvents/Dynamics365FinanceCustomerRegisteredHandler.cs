// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerRegisteredHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerEventHandler.
/// Implements the <see cref="Application.Events.IntegrationEventHandlerBase{Bspk.Customers.Infrastructure.IntegrationEvents.FFYCustomerRegisteredBusinessEvent}" />.
/// </summary>
/// <seealso cref="Application.Events.IntegrationEventHandlerBase{Bspk.Customers.Infrastructure.IntegrationEvents.FFYCustomerRegisteredBusinessEvent}" />
public class Dynamics365FinanceCustomerRegisteredHandler : IntegrationEventHandlerBase<Dynamics365FinanceCustomerRegistered>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<Dynamics365FinanceCustomerRegisteredHandler> _logger;
    private readonly string _originId;
    private readonly string _partitionId;
    private readonly IOptions<OrganizationSettings> _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerRegisteredHandler"/> class.
    /// </summary>
    /// <param name="erpState">State of the erp.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public Dynamics365FinanceCustomerRegisteredHandler(
        IDateTimeService dateTimeService,
        IOptions<OrganizationSettings> settings,
        ILogger<Dynamics365FinanceCustomerRegisteredHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultPartitionId);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultOriginId);
        _dateTimeService = dateTimeService;
        _settings = settings;
        _logger = logger;
        _partitionId = settings.Value.DefaultPartitionId;
        _originId = settings.Value.DefaultOriginId;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(Dynamics365FinanceCustomerRegistered baseEvent, CancellationToken cancellationToken)
        => await Task.FromResult<IEnumerable<BaseCommand>>(Array.Empty<BaseCommand>()).ConfigureAwait(false);

    /*
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(@event.Contact);
        ArgumentNullException.ThrowIfNull(@event.Contact.PostalAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(@event.Account);
        ArgumentException.ThrowIfNullOrWhiteSpace(@event.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(@event.BusinessEventLegalEntity);
        string? aggregateId = @event
            .ExternalReferences?
            .Where(p => p.SystemId == nameof(Hexalith))
            .FirstOrDefault()?
            .ExternalId;
        if (!string.IsNullOrWhiteSpace(aggregateId))
        {
            throw new InvalidOperationException($"Registrating customer '{aggregateId}' that already has an external id : {nameof(Hexalith)}='{aggregateId}'");
        }

        string originId = _originId;
        string customerId = @event.Account;
        string partitionId = _partitionId;
        string companyId = @event.BusinessEventLegalEntity.ToUpperInvariant();
        aggregateId = Customer.GetAggregateId(_partitionId, companyId, _originId, customerId);

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

        commands.Add(new RegisterCustomer(
                   partitionId,
                   companyId,
                   originId,
                   customerId,
                   @event.Name,
                   CustomerConverterHelper.ToPartyType(@event.PartyType ?? nameof(PartyType.Person)),
                   new(@event.Contact),
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

        return Task.FromResult<IEnumerable<BaseCommand>>(commands);
        */
}