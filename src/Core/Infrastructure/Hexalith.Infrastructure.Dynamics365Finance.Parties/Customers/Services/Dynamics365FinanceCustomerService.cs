// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 12-14-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-14-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Services;

using System;
using System.Linq;
using System.Threading.Tasks;

using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Filters;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Filters;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class Dynamics365FinanceCustomerService.
/// </summary>
public class Dynamics365FinanceCustomerService : IDynamics365FinanceCustomerService
{
    private readonly IDynamics365FinanceClient<CustomerBase> _customerBaseService;

    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerV3> _customerService;

    private readonly IDynamics365FinanceClient<CustomerExternalSystemCode> _externalCodeService;

    private readonly ILogger<Dynamics365FinanceCustomerService> _logger;

    /// <summary>
    /// The store service.
    /// </summary>
    private readonly IDynamics365FinanceClient<RetailStore> _storeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceCustomerService"/> class.
    /// </summary>
    /// <param name="customerBaseService">The customer base service.</param>
    /// <param name="customerService">The customer service.</param>
    /// <param name="externalCodeService">The external code service.</param>
    /// <param name="storeService">The store service.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public Dynamics365FinanceCustomerService(
        IDynamics365FinanceClient<CustomerBase> customerBaseService,
        IDynamics365FinanceClient<CustomerV3> customerService,
        IDynamics365FinanceClient<CustomerExternalSystemCode> externalCodeService,
        IDynamics365FinanceClient<RetailStore> storeService,
        ILogger<Dynamics365FinanceCustomerService> logger)
    {
        ArgumentNullException.ThrowIfNull(customerBaseService);
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(externalCodeService);
        ArgumentNullException.ThrowIfNull(storeService);
        ArgumentNullException.ThrowIfNull(logger);

        _customerBaseService = customerBaseService;
        _customerService = customerService;
        _externalCodeService = externalCodeService;
        _storeService = storeService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<string> CreateCustomerAsync(CustomerRegistered registered, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(registered);
        if (string.IsNullOrWhiteSpace(registered.WarehouseId))
        {
            throw new InvalidOperationException($"No warehouse defined for customer {registered.AggregateId}.");
        }

        try
        {
            // Find the customer in Dynamics 365 for Finance
            string? hexalithExternalCodeCustomer = await FindExternalCodeAsync(registered.CompanyId, nameof(Hexalith), registered.AggregateId, cancellationToken)
                    .ConfigureAwait(false);
            string? originExternalCodeCustomer = await FindExternalCodeAsync(registered.CompanyId, registered.OriginId, registered.Id, cancellationToken)
                    .ConfigureAwait(false);
            string tempName = $"[{registered.AggregateId}]";
            string? customerAccount = await FindAsync(registered.CompanyId, tempName, cancellationToken)
                    .ConfigureAwait(false);
            CustomerAccountKey customerKey;
            if (string.IsNullOrWhiteSpace(customerAccount))
            {
                // No incompleted customer found in Dynamics 365 Finance.
                if (string.IsNullOrWhiteSpace(hexalithExternalCodeCustomer) && string.IsNullOrWhiteSpace(originExternalCodeCustomer))
                {
                    CustomerV3 customer = await CreateCustomerV3Async(registered, tempName, cancellationToken)
                        .ConfigureAwait(false);
                    customerAccount = customer.CustomerAccount ?? throw new InvalidOperationException($"Created Dynamics 365 Finance customer account number is not defined for {registered.AggregateId}.");
                    customerKey = new(registered.CompanyId, customerAccount);
                }
                else
                {
                    // At least one external code is mapped to an existing customer, throw an error.
                    throw new InvalidOperationException($"Customer {registered.AggregateId} already exists in Dynamics 365 for Finance company {registered.CompanyId} : {hexalithExternalCodeCustomer} {originExternalCodeCustomer}");
                }
            }
            else
            {
                // An incompleted customer was found in Dynamics 365 Finance.
                customerKey = new(registered.CompanyId, customerAccount);
            }

            DateTimeOffset? birthDate = registered.Contact?.Person?.BirthDate;
            if (string.IsNullOrWhiteSpace(originExternalCodeCustomer))
            {
                _ = await _externalCodeService
                        .PostAsync(
                            new CustomerExternalSystemCode(
                                null,
                                registered.CompanyId,
                                registered.OriginId,
                                customerAccount,
                                registered.Id),
                            cancellationToken).ConfigureAwait(false);
            }

            if (string.IsNullOrWhiteSpace(hexalithExternalCodeCustomer))
            {
                _ = await _externalCodeService
                        .PostAsync(
                            new CustomerExternalSystemCode(
                            null,
                            registered.CompanyId,
                            nameof(Hexalith),
                            customerAccount,
                            registered.AggregateId),
                            cancellationToken).ConfigureAwait(false);
            }

            await _customerBaseService.PatchAsync(
                customerKey,
                new CustomerBase(
                    registered.CompanyId,
                    customerAccount,
                    null,
                    registered.Name,
                    registered.Contact?.Person?.Title,
                    birthDate?.Day,
                    birthDate?.Month,
                    birthDate?.Year),
                cancellationToken).ConfigureAwait(false);
            return customerAccount;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error while creating customer {registered.AggregateId}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<CustomerV3> CreateCustomerV3Async(CustomerRegistered registered, string? temporaryName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(registered);
        if (string.IsNullOrWhiteSpace(registered.WarehouseId))
        {
            throw new InvalidOperationException($"No warehouse defined for customer {registered.AggregateId}.");
        }

        try
        {
            CustomerV3 customerTemplate = await GetStoreDefaultCustomerTemplateAsync(registered.WarehouseId, cancellationToken)
                .ConfigureAwait(false);
            CustomerV3Create create = registered
                .ToDynamics365FinanceCustomerCreate(temporaryName, customerTemplate);
            CustomerV3 customer = await _customerService
                .PostAsync(create, cancellationToken)
                .ConfigureAwait(false);
            return customer;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error while creating customer {registered.AggregateId}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<CustomerV3> GetStoreDefaultCustomerTemplateAsync(string warehouseId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(warehouseId);

        // Get customer default values from the store and it's related customer template.
        RetailStore[] stores = (await _storeService
                .GetAsync(new RetailStoreByWarehouseFilter(warehouseId), cancellationToken)
                .ConfigureAwait(false))
                .ToArray();
        if (stores.Length < 1)
        {
            // No store found for the warehouse.
            throw new InvalidOperationException($"No store found for warehouse {warehouseId} while getting customer default values.");
        }

        if (stores.Length > 1)
        {
            // Duplicate stores found for the warehouse.
            throw new InvalidOperationException($"Duplicate store found for warehouse {warehouseId} while getting customer default values : {string.Join(';', stores.Select(s => s.RetailChannelId))}");
        }

        RetailStore store = stores[0];

        if (string.IsNullOrWhiteSpace(store.DefaultCustomerAccount))
        {
            // No default customer template defined for the store.
            throw new InvalidOperationException($"No default customer template defined for store {store.RetailChannelId} while getting customer default values.");
        }

        if (string.IsNullOrWhiteSpace(store.DefaultCustomerLegalEntity))
        {
            // No default legal entity defined for the store.
            throw new InvalidOperationException($"No default customer legal entity defined for store {store.RetailChannelId} while getting customer default values.");
        }

        return await _customerService
                .GetSingleAsync(
                    new CustomerAccountKey(
                    store.DefaultCustomerLegalEntity,
                    store.DefaultCustomerAccount),
                    cancellationToken)
                .ConfigureAwait(false);
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
}