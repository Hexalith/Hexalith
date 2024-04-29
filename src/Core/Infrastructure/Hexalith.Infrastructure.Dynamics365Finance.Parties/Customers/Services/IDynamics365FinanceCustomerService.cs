// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 12-14-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-14-2023
// ***********************************************************************
// <copyright file="IDynamics365FinanceCustomerService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Services;

using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

/// <summary>
/// Interface IDynamics365FinanceCustomerService.
/// </summary>
public interface IDynamics365FinanceCustomerService
{
    /// <summary>
    /// Creates the customer asynchronous.
    /// </summary>
    /// <param name="registered">The registered.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.String&gt;.</returns>
    Task<string> CreateCustomerAsync(CustomerRegistered registered, CancellationToken cancellationToken);

    /// <summary>
    /// Creates the customer asynchronous.
    /// </summary>
    /// <param name="registered">The registered.</param>
    /// <param name="temporaryName">Name of the temporary.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>System.Threading.Tasks.Task&lt;Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities.CustomerV3&gt;.</returns>
    Task<CustomerV3> CreateCustomerV3Async(CustomerRegistered registered, string? temporaryName, CancellationToken cancellationToken);

    /// <summary>
    /// Finds the customer asynchronous.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CustomerV3&gt;&gt;.</returns>
    Task<CustomerV3?> FindCustomerAsync(string companyId, string id, CancellationToken cancellationToken);

    /// <summary>
    /// Finds the customer by external identifier in the specified company.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="system">The system.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.ValueTuple&lt;System.Nullable&lt;CustomerV3&gt;, System.Nullable&lt;System.String&gt;&gt;&gt;.</returns>
    Task<CustomerV3?> FindCustomerByExternalIdAsync(string companyId, string system, string externalId, CancellationToken cancellationToken);

    /// <summary>
    /// Finds the customer by external identifier in any company.
    /// </summary>
    /// <param name="system">The system.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.ValueTuple&lt;System.Nullable&lt;CustomerV3&gt;, System.Nullable&lt;System.String&gt;&gt;&gt;.</returns>
    Task<CustomerV3?> FindCustomerByExternalIdAsync(string system, string externalId, CancellationToken cancellationToken);

    /// <summary>
    /// Get template customer as an asynchronous operation.
    /// </summary>
    /// <param name="warehouseId">The warehouse identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;CustomerV3&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.InvalidOperationException">Warehouse is not defined.</exception>
    /// <exception cref="System.InvalidOperationException">No store found for warehouse {warehouseId} while getting customer default values.</exception>
    /// <exception cref="System.InvalidOperationException">Duplicate store found for warehouse {warehouseId} while getting customer default values : {string.Join(';', stores.Select(s =&gt; s.RetailChannelId))}.</exception>
    /// <exception cref="System.InvalidOperationException">No default customer template defined for store {store.RetailChannelId} while getting customer default values.</exception>
    /// <exception cref="System.InvalidOperationException">No default customer legal entity defined for store {store.RetailChannelId} while getting customer default values.</exception>
    Task<CustomerV3> GetStoreDefaultCustomerTemplateAsync(string warehouseId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the customer asynchronous.
    /// </summary>
    /// <param name="changed">The changed.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task UpdateCustomerAsync(CustomerInformationChanged changed, CancellationToken cancellationToken);
}