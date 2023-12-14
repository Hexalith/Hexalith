// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 11-18-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-13-2023
// ***********************************************************************
// <copyright file="CustomerRegisteredHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;

using System.Threading;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Filters;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Filters;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerRegisteredHandler.
/// Implements the <see cref="CustomerEventsHandler{CustomerRegistered}" />.
/// </summary>
/// <seealso cref="CustomerEventsHandler{CustomerRegistered}" />
public class CustomerRegisteredHandler : IntegrationEventHandler<CustomerRegistered>
{
    /// <summary>
    /// The customer base service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerBase> _customerBaseService;

    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerV3> _customerService;

    /// <summary>
    /// The external code service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerExternalSystemCode> _externalCodeService;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<CustomerRegisteredHandler> _logger;

    /// <summary>
    /// The organization settings.
    /// </summary>
    private readonly IOptions<OrganizationSettings> _organizationSettings;

    /// <summary>
    /// The origin identifier.
    /// </summary>
    private readonly string? _originId;

    private readonly IDynamics365FinanceClient<RetailStore> _storeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRegisteredHandler"/> class.
    /// </summary>
    /// <param name="customerBaseService">The customer base service.</param>
    /// <param name="customerService">The customer service.</param>
    /// <param name="externalCodeService">The external code service.</param>
    /// <param name="storeService">The store service.</param>
    /// <param name="organizationSettings">The organization settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public CustomerRegisteredHandler(
        IDynamics365FinanceClient<CustomerBase> customerBaseService,
        IDynamics365FinanceClient<CustomerV3> customerService,
        IDynamics365FinanceClient<CustomerExternalSystemCode> externalCodeService,
        IDynamics365FinanceClient<RetailStore> storeService,
        IOptions<OrganizationSettings> organizationSettings,
        ILogger<CustomerRegisteredHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(customerBaseService);
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(externalCodeService);
        ArgumentNullException.ThrowIfNull(storeService);
        ArgumentNullException.ThrowIfNull(organizationSettings);
        ArgumentNullException.ThrowIfNull(logger);

        ArgumentException.ThrowIfNullOrEmpty(organizationSettings.Value.DefaultOriginId);

        _customerBaseService = customerBaseService;
        _customerService = customerService;
        _externalCodeService = externalCodeService;
        _storeService = storeService;
        _organizationSettings = organizationSettings;
        _logger = logger;
        _originId = _organizationSettings.Value.DefaultOriginId;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(CustomerRegistered @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);
        if (@event.OriginId.Equals(_originId, StringComparison.OrdinalIgnoreCase))
        {
            return [];
        }

        // Find the customer in Dynamics 365 for Finance
        string? hexalithExternalCodeCustomer = await FindExternalCodeAsync(@event.CompanyId, nameof(Hexalith), @event.AggregateId, cancellationToken)
                .ConfigureAwait(false);
        string? originExternalCodeCustomer = await FindExternalCodeAsync(@event.CompanyId, @event.OriginId, @event.Id, cancellationToken)
                .ConfigureAwait(false);
        string tempName = $"[{@event.AggregateId}]";
        string? customerAccount = await FindAsync(@event.CompanyId, tempName, cancellationToken)
                .ConfigureAwait(false);
        CustomerAccountKey customerKey;
        if (string.IsNullOrWhiteSpace(customerAccount))
        {
            // No incompleted customer found in Dynamics 365 Finance.
            if (string.IsNullOrWhiteSpace(hexalithExternalCodeCustomer) && string.IsNullOrWhiteSpace(originExternalCodeCustomer))
            {
                CustomerV3 customerV3 = await GetStoreTemplateCustomerAsync(@event.AggregateId, @event.CompanyId, @event.WarehouseId, cancellationToken)
                        .ConfigureAwait(false);
                CustomerV3Create create = @event.ToDynamics365FinanceCustomerCreate(tempName);
                CustomerV3 customer = await _customerService
                        .PostAsync(create, cancellationToken)
                        .ConfigureAwait(false);
                customerAccount = customer.CustomerAccount;
                customerKey = new(@event.CompanyId, customerAccount);
            }
            else
            {
                // At least one external code is mapped to an existing customer, throw an error.
                throw new InvalidOperationException($"Customer {@event.AggregateId} already exists in Dynamics 365 for Finance company {@event.CompanyId} : {hexalithExternalCodeCustomer} {originExternalCodeCustomer}");
            }
        }
        else
        {
            // An incompleted customer was found in Dynamics 365 Finance.
            customerKey = new(@event.CompanyId, customerAccount);
        }

        DateTimeOffset? birthDate = @event.Contact?.Person?.BirthDate;
        if (string.IsNullOrWhiteSpace(originExternalCodeCustomer))
        {
            _ = await _externalCodeService
                    .PostAsync(
                        new CustomerExternalSystemCode(
                            null,
                            @event.CompanyId,
                            @event.OriginId,
                            customerAccount,
                            @event.Id),
                        cancellationToken).ConfigureAwait(false);
        }

        if (string.IsNullOrWhiteSpace(hexalithExternalCodeCustomer))
        {
            _ = await _externalCodeService
                    .PostAsync(
                        new CustomerExternalSystemCode(
                        null,
                        @event.CompanyId,
                        nameof(Hexalith),
                        customerAccount,
                        @event.AggregateId),
                        cancellationToken).ConfigureAwait(false);
        }

        await _customerBaseService.PatchAsync(
            customerKey,
            new CustomerBase(
                @event.CompanyId,
                @event.Id,
                null,
                @event.Name,
                @event.Contact?.Person?.Title,
                birthDate?.Day,
                birthDate?.Month,
                birthDate?.Year),
            cancellationToken).ConfigureAwait(false);
        return [];
    }

    /// <summary>
    /// Find as an asynchronous operation.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="tempName">Name of the temporary.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;string?&gt; representing the asynchronous operation.</returns>
    private async Task<string?> FindAsync(string companyId, string tempName, CancellationToken cancellationToken)
    {
        IEnumerable<CustomerBase> result = await _customerBaseService.GetAsync(
                    new CustomerByNameFilter(companyId, tempName),
                    cancellationToken).ConfigureAwait(false);
        return result.FirstOrDefault()?.CustomerAccount;
    }

    /// <summary>
    /// Find external code as an asynchronous operation.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;string?&gt; representing the asynchronous operation.</returns>
    private async Task<string?> FindExternalCodeAsync(string companyId, string originId, string externalId, CancellationToken cancellationToken)
    {
        IEnumerable<CustomerExternalSystemCode> externalCodes = await _externalCodeService
             .GetAsync(
                 new CustomerExternalCodeFilter(
                 companyId,
                 originId,
                 externalId),
                 cancellationToken).ConfigureAwait(false);
        return externalCodes.FirstOrDefault()?.CustomerAccountNumber;
    }

    private async Task<CustomerV3> GetStoreTemplateCustomerAsync(string aggregateId, string companyId, string? warehouseId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(warehouseId))
        {
            throw new InvalidOperationException($"Warehouse is not defined for customer {aggregateId}.");
        }

        // Get customer default values from the store and it's related customer template.
        RetailStore[] stores = (await _storeService
                .GetAsync(new RetailStoreByWarehouseFilter(companyId, warehouseId), cancellationToken)
                .ConfigureAwait(false))
                .ToArray();
        if (stores.Length < 1)
        {
            // No store found for the warehouse.
            throw new InvalidOperationException($"No store found for warehouse {warehouseId} while getting customer '{aggregateId}' default values.");
        }

        if (stores.Length > 1)
        {
            // Duplicate stores found for the warehouse.
            throw new InvalidOperationException($"Duplicate store found for warehouse {warehouseId} while getting customer '{aggregateId}' default values : {string.Join(';', stores.Select(s => s.RetailChannelId))}");
        }

        RetailStore store = stores[0];

        if (string.IsNullOrWhiteSpace(store.DefaultCustomerAccount))
        {
            // No default customer template defined for the store.
            throw new InvalidOperationException($"No default customer template defined for store {store.RetailChannelId} while getting customer '{aggregateId}' default values.");
        }

        if (string.IsNullOrWhiteSpace(store.DefaultCustomerLegalEntity))
        {
            // No default legal entity defined for the store.
            throw new InvalidOperationException($"No default customer legal entity defined for store {store.RetailChannelId} while getting customer '{aggregateId}' default values.");
        }

        return await _customerService
                .GetSingleAsync(
                    new CustomerAccountKey(
                    store.DefaultCustomerLegalEntity,
                    store.DefaultCustomerAccount),
                    cancellationToken)
                .ConfigureAwait(false);
    }
}