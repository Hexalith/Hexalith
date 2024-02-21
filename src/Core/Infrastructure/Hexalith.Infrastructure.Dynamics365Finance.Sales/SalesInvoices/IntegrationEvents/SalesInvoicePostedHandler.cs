// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Sales
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="SalesInvoicePostedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.IntegrationEvents;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class SalesInvoiceEventHandler.
/// Implements the <see cref="IntegrationEventHandler{Bspk.SalesInvoices.Infrastructure.IntegrationEvents.FFYSalesInvoiceRegisteredBusinessEvent}" />.
/// </summary>
/// <seealso cref="IntegrationEventHandler{Bspk.SalesInvoices.Infrastructure.IntegrationEvents.FFYSalesInvoiceRegisteredBusinessEvent}" />
public class SalesInvoicePostedHandler : IntegrationEventHandler<SalesInvoicePostedBusinessEvent>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<SalesInvoicePostedHandler> _logger;
    private readonly string _originId;
    private readonly string _partitionId;
    private readonly IOptions<OrganizationSettings> _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoicePostedHandler"/> class.
    /// </summary>
    /// <param name="erpState">State of the erp.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    public SalesInvoicePostedHandler(
        IDateTimeService dateTimeService,
        IOptions<OrganizationSettings> settings,
        ILogger<SalesInvoicePostedHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.Value.DefaultPartitionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.Value.DefaultOriginId);
        _dateTimeService = dateTimeService;
        _settings = settings;
        _logger = logger;
        _partitionId = settings.Value.DefaultPartitionId;
        _originId = settings.Value.DefaultOriginId;
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseCommand>> ApplyAsync(SalesInvoicePostedBusinessEvent @event, CancellationToken cancellationToken)
        => Task.FromResult<IEnumerable<BaseCommand>>(Array.Empty<BaseCommand>());

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
        aggregateId = SalesInvoice.GetAggregateId(_partitionId, companyId, _originId, customerId);

        string customerAggregateName = SalesInvoice.GetAggregateName();
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

        commands.Add(new RegisterSalesInvoice(
                   partitionId,
                   companyId,
                   originId,
                   customerId,
                   @event.Name,
                   SalesInvoiceConverterHelper.ToPartyType(@event.PartyType ?? nameof(PartyType.Person)),
                   new(@event.Contact),
                   @event.WarehouseId,
                   @event.CommissionSalesGroupId,
                   @event.GroupId,
                   @event.SalesCurrencyId,
                   _dateTimeService.UtcNow));

        bool directDelivery = @event.InterCompanyDirectDelivery == "Yes";
        if (directDelivery)
        {
            commands.Add(new SelectIntercompanyDropshipDeliveryForSalesInvoice(_partitionId, companyId, _originId, customerId));
        }
        else
        {
            commands.Add(new DeselectIntercompanyDropshipDeliveryForSalesInvoice(_partitionId, companyId, _originId, customerId));
        }

        return Task.FromResult<IEnumerable<BaseCommand>>(commands);
        */
}