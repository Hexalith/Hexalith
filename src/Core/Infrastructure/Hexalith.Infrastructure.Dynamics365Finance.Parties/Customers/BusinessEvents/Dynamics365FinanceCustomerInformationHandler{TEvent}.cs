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
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Options;

/// <summary>
/// Class FFYCustomerInformationBusinessEventHandler.
/// Implements the <see cref="IntegrationEventHandler`1" />.
/// </summary>
/// <typeparam name="TEvent">The type of the t event.</typeparam>
/// <seealso cref="IntegrationEventHandler`1" />
public abstract class Dynamics365FinanceCustomerInformationHandler<TEvent> : IntegrationEventHandler<TEvent>
    where TEvent : Dynamics365FinanceCustomerInformationBusinessEvent
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// The external reference service.
    /// </summary>
 // private readonly IExternalReferenceMapperService _externalReferenceMapperService;
    private readonly string _partitionId;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerInformationHandler{TEvent}"/> class.
    /// </summary>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected Dynamics365FinanceCustomerInformationHandler(
        IDateTimeService dateTimeService,
        IOptions<OrganizationSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(dateTimeService);

        // ArgumentNullException.ThrowIfNull(externalReferenceMapperService);
        ArgumentNullException.ThrowIfNull(settings);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultPartitionId);
        _partitionId = settings.Value.DefaultPartitionId;
        _dateTimeService = dateTimeService;

        // _externalReferenceMapperService = externalReferenceMapperService;
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseCommand>> ApplyAsync(TEvent @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(@event.Contact);
        ArgumentNullException.ThrowIfNull(@event.Contact.PostalAddress);
        ArgumentException.ThrowIfNullOrEmpty(@event.Account);
        ArgumentException.ThrowIfNullOrEmpty(@event.Name);
        ArgumentException.ThrowIfNullOrEmpty(@event.BusinessEventLegalEntity);
        string companyId = @event.BusinessEventLegalEntity.ToUpperInvariant();
        string customerId = @event.Account.ToUpperInvariant();
        string customerAggregateId = Customer.GetAggregateId(_partitionId, companyId, customerId);
        string customerAggregateName = Customer.GetAggregateName();
        List<BaseCommand> commands = [];
        if (@event.ExternalReferences != null)
        {
            foreach (ExternalReference reference in @event.ExternalReferences)
            {
                /*
                if (await _externalReferenceMapperService
                    .GetExternalIdAsync(
                        customerAggregateName,
                        customerAggregateId,
                        reference.SystemId,
                        cancellationToken)
                    .ConfigureAwait(false) != reference.ExternalId)
                {
                    AddExternalSystemReference mapExternalSystemReference = new(
                            _partitionId,
                            @event.BusinessEventLegalEntity,
                            reference.SystemId,
                            customerAggregateName,
                            reference.ExternalId,
                            customerAggregateId);
                    commands.Add(mapExternalSystemReference);
                }
                */
                AddExternalSystemReference mapExternalSystemReference = new(
                        _partitionId,
                        @event.BusinessEventLegalEntity,
                        reference.SystemId,
                        customerAggregateName,
                        reference.ExternalId,
                        customerAggregateId);
                commands.Add(mapExternalSystemReference);
            }
        }

        commands.Add(new RegisterOrChangeCustomer(
                   _partitionId,
                   companyId,
                   customerId,
                   @event.Name,
                   @event.Contact,
                   @event.WarehouseId,
                   @event.CommissionSalesGroupId,
                   _dateTimeService.UtcNow));

        bool directDelivery = @event.InterCompanyDirectDelivery == "Yes";
        if (directDelivery)
        {
            commands.Add(new SelectIntercompanyDropshipDeliveryForCustomer(_partitionId, companyId, customerId));
        }
        else
        {
            commands.Add(new DeselectIntercompanyDropshipDeliveryForCustomer(_partitionId, companyId, customerId));
        }

        return Task.FromResult<IEnumerable<BaseCommand>>(commands);
    }
}