// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 11-18-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-06-2023
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

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerChangedHandler.
/// Implements the <see cref="IntegrationEventHandler`1" />.
/// </summary>
/// <typeparam name="TCustomerEvent">The type of the t customer event.</typeparam>
/// <seealso cref="IntegrationEventHandler`1" />
public abstract partial class CustomerChangedHandler<TCustomerEvent> : IntegrationEventHandler<TCustomerEvent>
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

    private readonly ILogger<CustomerChangedHandler<TCustomerEvent>> _logger;

    /// <summary>
    /// The origin identifier.
    /// </summary>
    private readonly string _originId;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerChangedHandler{TCustomerEvent}"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="externalCustomerService">The external customer service.</param>
    /// <param name="externalReferenceMapperService">The external reference mapper service.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected CustomerChangedHandler(
        IDynamics365FinanceClient<CustomerV3> customerService,
        IDynamics365FinanceClient<CustomerExternalSystemCode> externalCustomerService,
        IExternalReferenceMapperService externalReferenceMapperService,
        IOptions<OrganizationSettings> settings,
        ILogger<CustomerChangedHandler<TCustomerEvent>> logger)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(externalCustomerService);
        ArgumentNullException.ThrowIfNull(externalReferenceMapperService);
        ArgumentNullException.ThrowIfNull(settings);
        SettingsException<OrganizationSettings>.ThrowIfNullOrEmpty(settings.Value.DefaultOriginId);
        _customerService = customerService;
        _externalCustomerService = externalCustomerService;
        _externalReferenceMapperService = externalReferenceMapperService;
        _logger = logger;
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
        string? customerId = await GetCustomerIdAsync(
            @event.PartitionId,
            @event.CompanyId,
            @event.OriginId,
            @event.Id,
            cancellationToken)
            .ConfigureAwait(false);

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
            CustomerExternalSystemCode ec = await _externalCustomerService.PostAsync(
                    new CustomerExternalSystemCode(
                        null,
                        @event.CompanyId,
                        @event.OriginId,
                        newCustomer.CustomerAccount,
                        @event.Id),
                    CancellationToken.None)
                .ConfigureAwait(false);
            if (!string.Equals(ec.System, @event.OriginId, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Posted customer external system code origin '{ec.System}' is invalid. Expected : {@event.OriginId}.");
            }

            if (!string.Equals(ec.ExternalCode, @event.Id, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Posted customer external system code '{ec.ExternalCode}' is invalid. Expected : {@event.Id}.");
            }

            if (!string.Equals(ec.DataAreaId, @event.CompanyId, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Posted customer external system code company '{ec.DataAreaId}' is invalid. Expected : {@event.CompanyId}.");
            }

            if (!string.Equals(ec.CustomerAccountNumber, newCustomer.CustomerAccount, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Posted customer external system code account '{ec.CustomerAccountNumber}' is invalid. Expected : {newCustomer.CustomerAccount}.");
            }
        }
        catch (Exception ex)
        {
            // Do not throw if external reference creation failed to avoid customer duplication
            LogCouldNotPostExternalCodeError(
                ex,
                @event.PartitionId,
                @event.CompanyId,
                @event.OriginId,
                @event.Id,
                newCustomer.CustomerAccount);
        }
#pragma warning restore CA1031 // Do not catch general exception types

        return [
            new AddExternalSystemReference(
                @event.PartitionId,
                @event.CompanyId,
                _originId,
                Customer.GetAggregateName(),
                newCustomer.CustomerAccount,
                @event.AggregateId)
            ];
    }

    /// <summary>
    /// Logs the could not post external code error.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="externalCustomerId">The external customer identifier.</param>
    /// <param name="customerId">The customer identifier.</param>
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Error,
        Message = "Could not post external code {OriginId}/{ExternalCustomerId} for Dynamics 365 Finance customer account {CustomerId} in company {CompanyId} and partition {PartitionId}.")]
    public partial void LogCouldNotPostExternalCodeError(
        Exception ex,
        string partitionId,
        string companyId,
        string originId,
        string externalCustomerId,
        string customerId);

    /// <summary>
    /// Get customer identifier as an asynchronous operation.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;string?&gt; representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">$"External customer {originId}/{customerId} is mapped to more than one customer account in Dynamics 365 Finance : {accounts}.</exception>
    private async Task<string?> GetCustomerIdAsync(
        string partitionId,
        string companyId,
        string originId,
        string customerId,
        CancellationToken cancellationToken)
    {
        if (_originId.Equals(originId, StringComparison.OrdinalIgnoreCase))
        {
            // Customer is already a Dynamics 365 Finance customer
            return customerId;
        }

        // Customer is not a Dynamics 365 Finance customer, find the Dynamics 365 Finance customer account number from the external reference
        string? id = await _externalReferenceMapperService
            .GetAggregateIdAsync(
                Customer.GetAggregateName(),
                partitionId,
                companyId,
                originId,
                customerId,
                cancellationToken)
            .ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(id))
        {
            // Customer is already a Dynamics 365 Finance customer, return the account number
            return id;
        }

        // Customer is not mapped, find if the customer identifier exists in the Dynamics 365 Finance external references
        CustomerExternalSystemCode[] externalCustomers = (await _externalCustomerService
                .GetAsync(
                    new CustomerExternalCodeFilter(
                    companyId,
                    originId,
                    customerId),
                    CancellationToken.None)
                .ConfigureAwait(false))
                .ToArray();
        if (externalCustomers.Length > 1)
        {
            string accounts = string.Join(';', externalCustomers.Select(p => p.CustomerAccountNumber));
            throw new InvalidOperationException($"External customer {originId}/{customerId} is mapped to more than one customer account in Dynamics 365 Finance : {accounts}");
        }

        return externalCustomers.FirstOrDefault()?.CustomerAccountNumber;
    }
}