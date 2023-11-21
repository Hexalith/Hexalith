// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 11-18-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-21-2023
// ***********************************************************************
// <copyright file="CustomerChangedHandler{TCustomerEvent}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.ExternalSystems.Commands;
using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Filters;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;

using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerChangedHandler.
/// Implements the <see cref="IntegrationEventHandler`1" />.
/// </summary>
/// <typeparam name="TCustomerEvent">The type of the t customer event.</typeparam>
/// <seealso cref="IntegrationEventHandler`1" />
public abstract class CustomerChangedHandler<TCustomerEvent> : IntegrationEventHandler<TCustomerEvent>
    where TCustomerEvent : CustomerEvent
{
    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerV3> _customerService;

    /// <summary>
    /// The external customer service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerExternalSystemCode> _externalCustomerService;

    /// <summary>
    /// The external reference mapper service.
    /// </summary>
    private readonly IExternalReferenceMapperService _externalReferenceMapperService;

    /// <summary>
    /// The origin identifier.
    /// </summary>
    private readonly string _originId;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerChangedHandler{TCustomerEvent}" /> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="externalCustomerService">The external customer service.</param>
    /// <param name="externalReferenceMapperService">The external reference mapper service.</param>
    /// <param name="settings">The settings.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected CustomerChangedHandler(
        IDynamics365FinanceClient<CustomerV3> customerService,
        IDynamics365FinanceClient<CustomerExternalSystemCode> externalCustomerService,
        IExternalReferenceMapperService externalReferenceMapperService,
        IOptions<OrganizationSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(externalCustomerService);
        ArgumentNullException.ThrowIfNull(externalReferenceMapperService);
        ArgumentNullException.ThrowIfNull(settings);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultOriginId);
        _customerService = customerService;
        _externalCustomerService = externalCustomerService;
        _externalReferenceMapperService = externalReferenceMapperService;
        _originId = settings.Value.DefaultOriginId;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(TCustomerEvent @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);
        CustomerV3 customer = @event switch
        {
            CustomerInformationChanged changed => changed.ToDynamics365FinanceCustomer(),
            CustomerRegistered registered => registered.ToDynamics365FinanceCustomer(),
            _ => throw new ArgumentException($"Event {@event.GetType().Name} is not a valid customer event. Expected : {nameof(CustomerRegistered)}; {nameof(CustomerInformationChanged)}.", nameof(@event)),
        };
        string? customerId;
        if (_originId.Equals(@event.OriginId, StringComparison.OrdinalIgnoreCase))
        {
            customerId = @event.Id;
        }
        else
        {
            customerId = await _externalReferenceMapperService
                .GetAggregateIdAsync(
                    Customer.GetAggregateName(),
                    @event.PartitionId,
                    @event.CompanyId,
                    @event.OriginId,
                    @event.Id,
                    cancellationToken)
                .ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(customerId))
            {
                CustomerExternalSystemCode externalCustomer = await _externalCustomerService
                    .GetSingleAsync(new CustomerExternalCodeFilter(@event.CompanyId, @event.OriginId, @event.Id), CancellationToken.None)
                    .ConfigureAwait(false);
                customerId = externalCustomer.CustomerAccountNumber;
            }
        }

        if (!string.IsNullOrWhiteSpace(customerId))
        {
            await _customerService
                .PatchAsync(
                    new CustomerAccountKey(@event.CompanyId, customerId),
                    customer,
                    CancellationToken.None)
                .ConfigureAwait(false);
            return [];
        }

        CustomerV3 newCustomer = await _customerService
            .PostAsync(customer, CancellationToken.None)
            .ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(newCustomer.CustomerAccount))
        {
            throw new InvalidOperationException($"Dynamics 365 Finance customer created has an undefined account number: Company={@event.CompanyId} ExternalId={@event.OriginId}/{@event.Id}.");
        }
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            _ = _externalCustomerService.PostAsync(
                    new CustomerExternalSystemCode(
                        null,
                        @event.CompanyId,
                        @event.OriginId,
                        newCustomer.CustomerAccount,
                        @event.Id),
                    CancellationToken.None)
                .ConfigureAwait(false);
        }
        catch
        {
            // Do not throw if external reference creation failed to avoid customer duplication
        }
#pragma warning restore CA1031 // Do not catch general exception types

        return [new AddExternalSystemReference(
                @event.PartitionId,
                @event.CompanyId,
                _originId,
                Customer.GetAggregateName(),
                newCustomer.CustomerAccount,
                string.Empty)];
    }
}