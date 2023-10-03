// ***********************************************************************
// Assembly         : Bspk.Customers
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="FFYCustomerInformationBusinessEventHandler{TEvent}.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Application.Parties.Commands;
using Hexalith.Application.Parties.Services;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions.Common;

/// <summary>
/// Class FinOpsCustomerHandler.
/// Implements the <see cref="IntegrationEventHandler{TEvent}" />.
/// </summary>
/// <typeparam name="TEvent">The type of the t event.</typeparam>
/// <seealso cref="IntegrationEventHandler{TEvent}" />
public abstract class FFYCustomerInformationBusinessEventHandler<TEvent> : IntegrationEventHandler<TEvent>
    where TEvent : FFYCustomerInformationBusinessEvent
{
    /// <summary>
    /// The aggregate external reference service.
    /// </summary>
    private readonly IAggregateExternalReferenceQueryService _aggregateExternalReferenceService;

    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly ICustomerQueryService _customerService;

    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// The external reference service.
    /// </summary>
    private readonly IExternalSystemReferenceQueryService _externalReferenceService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FFYCustomerInformationBusinessEventHandler{TEvent}" /> class.
    /// </summary>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="customerService">The customer service.</param>
    /// <param name="externalReferenceService">The external reference service.</param>
    /// <param name="aggregateExternalReferenceService">The aggregate external reference service.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    protected FFYCustomerInformationBusinessEventHandler(
        IDateTimeService dateTimeService,
        ICustomerQueryService customerService,
        IExternalSystemReferenceQueryService externalReferenceService,
        IAggregateExternalReferenceQueryService aggregateExternalReferenceService)
    {
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(externalReferenceService);
        ArgumentNullException.ThrowIfNull(aggregateExternalReferenceService);
        _dateTimeService = dateTimeService;
        _customerService = customerService;
        _externalReferenceService = externalReferenceService;
        _aggregateExternalReferenceService = aggregateExternalReferenceService;
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
        string companyId = @event.BusinessEventLegalEntity.ToUpperInvariant();
        string customerId = @event.Account.ToUpperInvariant();
        string customerAggregateId = Customer.GetAggregateId(companyId, customerId);
        List<BaseCommand> commands = [];
        if (@event.ExternalReferences != null)
        {
            foreach (ExternalReference reference in @event.ExternalReferences)
            {
                AddAggregateExternalReference addAggregateExternalReference = new(
                        Customer.GetAggregateName(),
                        customerAggregateId,
                        reference.SystemId,
                        reference.ExternalId);
                if (await _aggregateExternalReferenceService
                    .GetExternalIdAsync(
                        addAggregateExternalReference.AggregateId,
                        reference.SystemId,
                        cancellationToken)
                    .ConfigureAwait(false) != reference.ExternalId)
                {
                    commands.Add(addAggregateExternalReference);
                }

                AddExternalSystemReference mapExternalSystemReference = new(
                        reference.SystemId,
                        Customer.GetAggregateName(),
                        reference.ExternalId,
                        customerAggregateId);
                if (await _externalReferenceService.GetAsync(
                    mapExternalSystemReference.AggregateId,
                    cancellationToken)
                    .ConfigureAwait(false) != customerAggregateId)
                {
                    commands.Add(mapExternalSystemReference);
                }
            }
        }

        if (!await _customerService
            .ExistAsync(customerAggregateId, cancellationToken)
            .ConfigureAwait(false))
        {
            RegisterCustomer registerCustomer = new(
                   companyId,
                   customerId,
                   @event.Name,
                   @event.Contact,
                   @event.WarehouseId,
                   @event.CommissionSalesGroupId,
                   _dateTimeService.UtcNow);
            commands.Add(registerCustomer);
        }
        else
        {
            ChangeCustomerInformation changeCustomer = new(
                   companyId,
                   customerId,
                   @event.Name,
                   @event.Contact,
                   @event.WarehouseId,
                   @event.CommissionSalesGroupId,
                   _dateTimeService.UtcNow);
            if (await _customerService
                .HasChangesAsync(changeCustomer, cancellationToken)
                .ConfigureAwait(false))
            {
                commands.Add(changeCustomer);
            }
        }

        return commands;
    }
}