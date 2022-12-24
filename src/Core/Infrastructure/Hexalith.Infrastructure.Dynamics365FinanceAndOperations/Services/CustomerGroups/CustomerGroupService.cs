// <copyright file="CustomerGroupService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.CustomerGroups;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Class CustomerGroupService.
/// Implements the <see cref="Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.Dynamics365FinanceService{Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.CustomerGroups.CustomerGroup, Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.CustomerGroups.CustomerGroupCreate}" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.Dynamics365FinanceService{Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.CustomerGroups.CustomerGroup, Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.CustomerGroups.CustomerGroupCreate}" />
public class CustomerGroupService : Dynamics365FinanceService<CustomerGroup, CustomerGroupCreate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerGroupService" /> class.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="logger">The logger.</param>
    public CustomerGroupService(
        IDynamics365FinanceAndOperationsClient<CustomerGroup> client,
        ILogger<CustomerGroupService> logger)
        : base(client, logger)
    {
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<CustomerGroup> results = await Client.GetAsync(
                new Dictionary<string, object>(
                StringComparer.Ordinal)
                {
                     { nameof(CustomerGroup.CustomerGroupId), id },
                },
                cancellationToken).ConfigureAwait(false);
            int count = results.Count();
            return count == 1 || (count == 0 ? false : throw new Exception($"Customer group '{id}' is not unique."));
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while determining if customer group '{id}' exists.", ex);
        }
    }

    /// <summary>
    /// Get as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;CustomerGroup&gt; representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Error while getting customer group '{id}'.</exception>
    public async Task<CustomerGroup> GetAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            return await Client.GetSingleAsync(
                new Dictionary<string, object>(
                StringComparer.Ordinal)
                {
                     { nameof(CustomerGroup.CustomerGroupId), id },
                },
                cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while getting customer group '{id}'.", ex);
        }
    }

    /// <summary>
    /// Update as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="update">The update.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Error while updating customer group '{id}'.</exception>
    public async Task UpdateAsync(string id, CustomerGroupUpdate update, CancellationToken cancellationToken)
    {
        try
        {
            await Client.PatchAsync(
                new Dictionary<string, object>(
                StringComparer.Ordinal)
                {
                     { nameof(CustomerGroup.CustomerGroupId), id },
                },
                update,
                cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while updating customer group '{id}'.", ex);
        }
    }
}
